using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering.LWRP
{
    /// <summary>
    ///  Class <c>ScriptableRenderer</c> implements a rendering strategy. It describes how culling and lighting works and
    /// the effects supported.
    /// 
    ///  A renderer can be used for all cameras or be overridden on a per-camera basis. It will implement light culling and setup
    /// and describe a list of <c>ScriptableRenderPass</c> to execute in a frame. The renderer can be extended to support more effect with additional
    ///  <c>ScriptableRendererFeature</c>. Resources for the renderer are serialized in <c>ScriptableRendererData</c>.
    /// 
    /// he renderer resources are serialized in <c>ScriptableRendererData</c>. 
    /// <seealso cref="ScriptableRendererData"/>
    /// <seealso cref="ScriptableRendererFeature"/>
    /// <seealso cref="ScriptableRenderPass"/>
    /// </summary>
    public abstract class ScriptableRenderer
    {
        const int k_DepthStencilBufferBits = 32;
        public RenderTargetHandle cameraColorHandle { get; set; }
        public RenderTargetHandle cameraDepthHandle { get; set; }

        protected List<ScriptableRendererFeature> rendererFeatures
        {
            get => m_RendererFeatures;
        }

        protected List<ScriptableRenderPass> activeRenderPassQueue
        {
            get => m_ActiveRenderPassQueue;
        }

        List<ScriptableRenderPass> m_ActiveRenderPassQueue = new List<ScriptableRenderPass>(32);
        List<ScriptableRendererFeature> m_RendererFeatures = new List<ScriptableRendererFeature>(10);

        const string k_SetupRendering = "Setup Rendering";
        const string k_SetRenderTarget = "Set RenderTarget";
        const string k_ReleaseResourcesTag = "Release Resources";

        static RenderTargetIdentifier m_ActiveColorAttachment;
        static RenderTargetIdentifier m_ActiveDepthAttachment;

        bool m_FirstCameraRenderPassExecuted;

        internal static void ConfigureActiveTarget(RenderTargetIdentifier colorAttachment,
            RenderTargetIdentifier depthAttachment)
        {
            m_ActiveColorAttachment = colorAttachment;
            m_ActiveDepthAttachment = depthAttachment;
        }
        
        public ScriptableRenderer(ScriptableRendererData data)
        {
            m_RendererFeatures.AddRange(data.rendererFeatures.Where(x => x != null));
            cameraColorHandle = RenderTargetHandle.CameraTarget;
            cameraDepthHandle = RenderTargetHandle.CameraTarget;
            m_ActiveColorAttachment = BuiltinRenderTextureType.CameraTarget;
            m_ActiveDepthAttachment = BuiltinRenderTextureType.CameraTarget;
        }

        /// <summary>
        /// Configures the render passes that will execute for this renderer.
        /// This method is called per-camera every frame.
        /// </summary>
        /// <param name="renderingData">Current render state information.</param>
        /// <seealso cref="ScriptableRenderPass"/>
        /// <seealso cref="ScriptableRendererFeature"/>
        public abstract void Setup(ref RenderingData renderingData);

        /// <summary>
        /// Override this method to implement the lighting setup for the renderer. You can use this to 
        /// compute and upload light CBUFFER for example.
        /// </summary>
        /// <param name="context">Use this render context to issue any draw commands during execution.</param>
        /// <param name="renderingData">Current render state information.</param>
        public virtual void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
        {
        }

        /// <summary>
        /// Override this method to configure the culling parameters for the renderer. You can use this to configure if
        /// lights should be culled per-object or the maximum shadow distance for example.
        /// </summary>
        /// <param name="cullingParameters">Use this to change culling parameters used by the render pipeline.</param>
        /// <param name="cameraData">Current render state information.</param>
        public virtual void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters,
            ref CameraData cameraData)
        {
        }

        /// <summary>
        /// Execute the enqueued render passes. This automatically handles editor and stereo rendering.
        /// </summary>
        /// <param name="context">Use this render context to issue any draw commands during execution.</param>
        /// <param name="renderingData">Current render state information.</param>
        public void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            Camera camera = renderingData.cameraData.camera;
            ref CameraData cameraData = ref renderingData.cameraData;

            SetupRendering(context, ref cameraData);
            SortStable(m_ActiveRenderPassQueue);

            // Before Render Block. This render blocks always execute in mono rendering.
            // Camera is not setup. Lights are not setup.
            // Used to render input textures like shadowmaps.
            ExecuteBlock(RenderPassEvent.BeforeRendering, RenderPassEvent.BeforeRenderingPrepasses, context, ref renderingData, false);

            /// Configure shader variables and other unity properties that are required for rendering.
            /// * Setup Camera RenderTarget and Viewport
            /// * VR Camera Setup and SINGLE_PASS_STEREO props
            /// * Setup camera view, projection and their inverse matrices.
            /// * Setup properties: _WorldSpaceCameraPos, _ProjectionParams, _ScreenParams, _ZBufferParams, unity_OrthoParams
            /// * Setup camera world clip planes properties
            /// * Setup HDR keyword
            /// * Setup global time properties (_Time, _SinTime, _CosTime)
            bool stereoEnabled = cameraData.isStereoEnabled;
            context.SetupCameraProperties(camera, stereoEnabled);
            SetupLights(context, ref renderingData);

            if (stereoEnabled)
                BeginXRRendering(context, camera);

            // In this block stereo, camera matrices and lighting is setup, but camera target textures are not setup yet.
            // Use this to render prepasses that require stereo or camera matrices setup like depth prepass or screenspace shadow resolve.
            ExecuteBlock(RenderPassEvent.BeforeRenderingPrepasses, RenderPassEvent.BeforeRenderingOpaques , context, ref renderingData, stereoEnabled);
            
            // In this block main rendering executes.
            ExecuteBlock(RenderPassEvent.BeforeRenderingOpaques, RenderPassEvent.AfterRenderingPostProcessing, context, ref renderingData, stereoEnabled);

            DrawGizmos(context, camera, GizmoSubset.PreImageEffects);

            // In this block after rendering drawing happens, e.g, post processing, video player capture.
            ExecuteBlock(RenderPassEvent.AfterRenderingPostProcessing, (RenderPassEvent)Int32.MaxValue, context, ref renderingData, stereoEnabled);

            if (stereoEnabled)
                EndXRRendering(context, camera);

            DrawGizmos(context, camera, GizmoSubset.PostImageEffects);

            FinishRendering(context);
        }

        /// <summary>
        /// Enqueues a render pass for execution.
        /// </summary>
        /// <param name="pass">Render pass to be enqueued.</param>
        public void EnqueuePass(ScriptableRenderPass pass)
        {
            m_ActiveRenderPassQueue.Add(pass);
        }

        /// <summary>
        /// Returns a clear flag based on CameraClearFlags.
        /// </summary>
        /// <param name="cameraClearFlags">Camera clear flags.</param>
        /// <returns>A clear flag that tells if color and/or depth should be cleared.</returns>
        protected static ClearFlag GetCameraClearFlag(CameraClearFlags cameraClearFlags)
        {
#if UNITY_EDITOR
            // We need public API to tell if FrameDebugger is active and enabled. In that case
            // we want to force a clear to see properly the drawcall stepping.
            // For now, to fix FrameDebugger in Editor, we force a clear. 
            cameraClearFlags = CameraClearFlags.SolidColor;
#endif

            // LWRP doesn't support CameraClearFlags.DepthOnly and CameraClearFlags.Nothing.
            // CameraClearFlags.DepthOnly has the same effect of CameraClearFlags.SolidColor
            // CameraClearFlags.Nothing clears Depth on PC/Desktop and in mobile it clears both
            // depth and color.
            // CameraClearFlags.Skybox clears depth only.

            // Implementation details:
            // Camera clear flags are used to initialize the attachments on the first render pass.
            // ClearFlag is used together with Tile Load action to figure out how to clear the camera render target.
            // In Tile Based GPUs ClearFlag.Depth + RenderBufferLoadAction.DontCare becomes DontCare load action.
            // While ClearFlag.All + RenderBufferLoadAction.DontCare become Clear load action.
            // In mobile we force ClearFlag.All as DontCare doesn't have noticeable perf. difference from Clear
            // and this avoid tile clearing issue when not rendering all pixels in some GPUs.
            // In desktop/consoles there's actually performance difference between DontCare and Clear.

            // RenderBufferLoadAction.DontCare in PC/Desktop behaves as not clearing screen
            // RenderBufferLoadAction.DontCare in Vulkan/Metal behaves as DontCare load action
            // RenderBufferLoadAction.DontCare in GLES behaves as glInvalidateBuffer

            // Always clear on first render pass in mobile as it's same perf of DontCare and avoid tile clearing issues.
            if (Application.isMobilePlatform)
                return ClearFlag.All;

            if ((cameraClearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null) ||
                cameraClearFlags == CameraClearFlags.Nothing)
                return ClearFlag.Depth;

            return ClearFlag.All;
        }

        void SetupRendering(ScriptableRenderContext context, ref CameraData cameraData)
        {
            m_FirstCameraRenderPassExecuted = false;

            // Keywords are enabled while executing passes.
            CommandBuffer cmd = CommandBufferPool.Get(k_SetupRendering);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.MainLightShadows);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.MainLightShadowCascades);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightsVertex);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightsPixel);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightShadows);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.SoftShadows);
            cmd.DisableShaderKeyword(ShaderKeywordStrings.MixedLightingSubtractive);

            if (cameraColorHandle != RenderTargetHandle.CameraTarget || cameraDepthHandle != RenderTargetHandle.CameraTarget)
                CreateCameraTextures(cmd, ref cameraData);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        void ExecuteBlock(RenderPassEvent startEvent, RenderPassEvent endEvent,
            ScriptableRenderContext context, ref RenderingData renderingData, bool isStereoBlock, bool submit = false)
        {
            int currIndex = m_ActiveRenderPassQueue.FindIndex(x => (x.renderPassEvent >= startEvent && x.renderPassEvent < endEvent));
            if (currIndex == -1)
                return;

            while (currIndex < m_ActiveRenderPassQueue.Count && m_ActiveRenderPassQueue[currIndex].renderPassEvent < endEvent)
            {
                var renderPass = m_ActiveRenderPassQueue[currIndex];
                ExecuteRenderPass(context, renderPass, ref renderingData, isStereoBlock);
                currIndex++;
            }

            if (submit)
                context.Submit();
        }

        void ExecuteRenderPass(ScriptableRenderContext context, ScriptableRenderPass renderPass, ref RenderingData renderingData, bool isStereo)
        {
            CommandBuffer cmd = CommandBufferPool.Get(k_SetRenderTarget);
            renderPass.Configure(cmd, renderingData.cameraData.cameraTargetDescriptor);

            RenderTargetIdentifier passColorAttachment = renderPass.colorAttachment;
            RenderTargetIdentifier passDepthAttachment = renderPass.depthAttachment;

            if (!m_FirstCameraRenderPassExecuted && passColorAttachment == cameraColorHandle.Identifier())
            {
                m_FirstCameraRenderPassExecuted = true;
                SetFirstCameraRenderPass(context, cmd, ref renderingData.cameraData);
            }
            else
            {
                // When render pass doesn't call ConfigureTarget we assume it's expected to render to camera target
                // which might be backbuffer or the framebuffer render textures. 
                if (!renderPass.overrideCameraTarget)
                {
                    passColorAttachment = cameraColorHandle.Identifier();
                    passDepthAttachment = cameraDepthHandle.Identifier();
                }

                // Only setup render target if current render pass attachments are different from the active ones
                if (passColorAttachment != m_ActiveColorAttachment || passDepthAttachment != m_ActiveDepthAttachment)
                {
                    m_ActiveColorAttachment = passColorAttachment;
                    m_ActiveDepthAttachment = passDepthAttachment;
                    
                    RenderBufferLoadAction colorLoadAction = renderPass.clearFlag != ClearFlag.None ?
                        RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load;

                    RenderBufferLoadAction depthLoadAction = CoreUtils.HasFlag(renderPass.clearFlag, ClearFlag.Depth) ?
                        RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load;

                    TextureDimension dimension = (isStereo) ? XRGraphics.eyeTextureDesc.dimension : TextureDimension.Tex2D;
                    SetRenderTarget(cmd, passColorAttachment, colorLoadAction, RenderBufferStoreAction.Store,
                        passDepthAttachment, depthLoadAction, RenderBufferStoreAction.Store, renderPass.clearFlag, renderPass.clearColor,
                        dimension);
                }
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            renderPass.Execute(context, ref renderingData);
        }

        void BeginXRRendering(ScriptableRenderContext context, Camera camera)
        {
            context.StartMultiEye(camera);
        }

        void EndXRRendering(ScriptableRenderContext context, Camera camera)
        {
            context.StopMultiEye(camera);
            context.StereoEndRender(camera);
        }

        void CreateCameraTextures(CommandBuffer cmd, ref CameraData cameraData)
        {
            var descriptor = cameraData.cameraTargetDescriptor;
            int msaaSamples = descriptor.msaaSamples;

            if (cameraColorHandle != RenderTargetHandle.CameraTarget)
            {
                bool useDepthRenderBuffer = cameraDepthHandle == RenderTargetHandle.CameraTarget;
                var colorDescriptor = descriptor;
                colorDescriptor.depthBufferBits = (useDepthRenderBuffer) ? k_DepthStencilBufferBits : 0;
                cmd.GetTemporaryRT(cameraColorHandle.id, colorDescriptor, FilterMode.Bilinear);
            }

            if (cameraDepthHandle != RenderTargetHandle.CameraTarget)
            {
                var depthDescriptor = descriptor;
                depthDescriptor.colorFormat = RenderTextureFormat.Depth;
                depthDescriptor.depthBufferBits = k_DepthStencilBufferBits;
                depthDescriptor.bindMS = msaaSamples > 1 && !SystemInfo.supportsMultisampleAutoResolve && (SystemInfo.supportsMultisampledTextures != 0);
                cmd.GetTemporaryRT(cameraDepthHandle.id, depthDescriptor, FilterMode.Point);
            }
        }

        // The first render pass is special. We don't load camera textures to main memory and 
        // figure out if we need to render occlusion mesh when in VR. 
        void SetFirstCameraRenderPass(ScriptableRenderContext context, CommandBuffer cmd, ref CameraData cameraData)
        {
            m_ActiveColorAttachment = cameraColorHandle.Identifier();
            m_ActiveDepthAttachment = cameraDepthHandle.Identifier();

            Camera camera = cameraData.camera;
            ClearFlag clearFlag = GetCameraClearFlag(camera.clearFlags);
            SetRenderTarget(cmd, cameraColorHandle.Identifier(), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                cameraDepthHandle.Identifier(), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, clearFlag,
                CoreUtils.ConvertSRGBToActiveColorSpace(camera.backgroundColor), cameraData.cameraTargetDescriptor.dimension);

            if (cameraData.isStereoEnabled)
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                context.StartMultiEye(camera);
                XRUtils.DrawOcclusionMesh(cmd, camera);
            }
        }

        void SetRenderTarget(
            CommandBuffer cmd,
            RenderTargetIdentifier colorAttachment,
            RenderBufferLoadAction colorLoadAction,
            RenderBufferStoreAction colorStoreAction,
            ClearFlag clearFlags,
            Color clearColor,
            TextureDimension dimension)
        {
            if (dimension == TextureDimension.Tex2DArray)
                CoreUtils.SetRenderTarget(cmd, colorAttachment, clearFlags, clearColor, 0, CubemapFace.Unknown, -1);
            else
                CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor);
        }

        void SetRenderTarget(
            CommandBuffer cmd,
            RenderTargetIdentifier colorAttachment,
            RenderBufferLoadAction colorLoadAction,
            RenderBufferStoreAction colorStoreAction,
            RenderTargetIdentifier depthAttachment,
            RenderBufferLoadAction depthLoadAction,
            RenderBufferStoreAction depthStoreAction,
            ClearFlag clearFlags,
            Color clearColor,
            TextureDimension dimension)
        {
            if (depthAttachment == BuiltinRenderTextureType.CameraTarget)
            {
                SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor,
                    dimension);
            }
            else
            {
                if (dimension == TextureDimension.Tex2DArray)
                    CoreUtils.SetRenderTarget(cmd, colorAttachment, depthAttachment,
                        clearFlags, clearColor, 0, CubemapFace.Unknown, -1);
                else
                    CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction,
                        depthAttachment, depthLoadAction, depthStoreAction, clearFlags, clearColor);
            }
        }

        [Conditional("UNITY_EDITOR")]
        void DrawGizmos(ScriptableRenderContext context, Camera camera, GizmoSubset gizmoSubset)
        {
#if UNITY_EDITOR
            if (UnityEditor.Handles.ShouldRenderGizmos())
                context.DrawGizmos(camera, gizmoSubset);
#endif
        }

        void FinishRendering(ScriptableRenderContext context)
        {
            CommandBuffer cmd = CommandBufferPool.Get(k_ReleaseResourcesTag);

            for (int i = 0; i < m_ActiveRenderPassQueue.Count; ++i)
                m_ActiveRenderPassQueue[i].FrameCleanup(cmd);

            if (cameraColorHandle != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(cameraColorHandle.id);
                cameraColorHandle = RenderTargetHandle.CameraTarget;
            }

            if (cameraDepthHandle != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(cameraDepthHandle.id);
                cameraDepthHandle = RenderTargetHandle.CameraTarget;
            }

            m_ActiveColorAttachment = BuiltinRenderTextureType.CameraTarget;
            m_ActiveDepthAttachment = BuiltinRenderTextureType.CameraTarget;

            m_ActiveRenderPassQueue.Clear();

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        internal static void SortStable(List<ScriptableRenderPass> list)
        {
            int j;
            for (int i = 1; i < list.Count; ++i)
            {
                ScriptableRenderPass curr = list[i];

                j = i - 1;
                for (; j >= 0 && curr < list[j]; --j)
                    list[j + 1] = list[j];

                list[j + 1] = curr;
            }
        }
    }
}
