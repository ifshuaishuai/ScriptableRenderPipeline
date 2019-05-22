using System;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public partial class RenderPipelineResources : ScriptableObject
    {
        [Serializable, ReloadGroup]
        public sealed class ShaderResources
        {
            // Defaults
            [Reload("Runtime/Material/Lit/Lit.shader")]
            public Shader defaultPS;

            // Debug
            [Reload("Runtime/Debug/DebugDisplayLatlong.Shader")]
            public Shader debugDisplayLatlongPS;
            [Reload("Runtime/Debug/DebugViewMaterialGBuffer.Shader")]
            public Shader debugViewMaterialGBufferPS;
            [Reload("Runtime/Debug/DebugViewTiles.Shader")]
            public Shader debugViewTilesPS;
            [Reload("Runtime/Debug/DebugFullScreen.Shader")]
            public Shader debugFullScreenPS;
            [Reload("Runtime/Debug/DebugColorPicker.Shader")]
            public Shader debugColorPickerPS;
            [Reload("Runtime/Debug/DebugLightVolumes.Shader")]
            public Shader debugLightVolumePS;
            [Reload("Runtime/Debug/DebugLightVolumes.compute")]
            public ComputeShader debugLightVolumeCS;

            // Lighting
            [Reload("Runtime/Lighting/Deferred.Shader")]
            public Shader deferredPS;
            [Reload("Runtime/RenderPipeline/RenderPass/ColorPyramidPS.Shader")]
            public Shader colorPyramidPS;
            [Reload("Runtime/RenderPipeline/RenderPass/DepthPyramid.compute")]
            public ComputeShader depthPyramidCS;
            [Reload("Runtime/Core/CoreResources/GPUCopy.compute")]
            public ComputeShader copyChannelCS;
            [Reload("Runtime/Lighting/ScreenSpaceLighting/ScreenSpaceReflections.compute")]
            public ComputeShader screenSpaceReflectionsCS;
            [Reload("Runtime/RenderPipeline/RenderPass/Distortion/ApplyDistortion.shader")]
            public Shader applyDistortionPS;

            // Lighting tile pass
            [Reload("Runtime/Lighting/LightLoop/cleardispatchindirect.compute")]
            public ComputeShader clearDispatchIndirectCS;
            [Reload("Runtime/Lighting/LightLoop/builddispatchindirect.compute")]
            public ComputeShader buildDispatchIndirectCS;
            [Reload("Runtime/Lighting/LightLoop/scrbound.compute")]
            public ComputeShader buildScreenAABBCS;
            [Reload("Runtime/Lighting/LightLoop/lightlistbuild.compute")]
            public ComputeShader buildPerTileLightListCS;               // FPTL
            [Reload("Runtime/Lighting/LightLoop/lightlistbuild-bigtile.compute")]
            public ComputeShader buildPerBigTileLightListCS;
            [Reload("Runtime/Lighting/LightLoop/lightlistbuild-clustered.compute")]
            public ComputeShader buildPerVoxelLightListCS;              // clustered
            [Reload("Runtime/Lighting/LightLoop/materialflags.compute")]
            public ComputeShader buildMaterialFlagsCS;
            [Reload("Runtime/Lighting/LightLoop/Deferred.compute")]
            public ComputeShader deferredCS;
            [Reload("Runtime/Lighting/Shadow/ContactShadows.compute")]
            public ComputeShader contactShadowCS;
            [Reload("Runtime/Lighting/VolumetricLighting/VolumeVoxelization.compute")]
            public ComputeShader volumeVoxelizationCS;
            [Reload("Runtime/Lighting/VolumetricLighting/VolumetricLighting.compute")]
            public ComputeShader volumetricLightingCS;
            [Reload("Runtime/Lighting/LightLoop/DeferredTile.shader")]
            public Shader deferredTilePS;
            [Reload("Runtime/Lighting/Shadow/ScreenSpaceShadows.shader")]
            public Shader screenSpaceShadowPS;

            [Reload("Runtime/Material/SubsurfaceScattering/SubsurfaceScattering.compute")]
            public ComputeShader subsurfaceScatteringCS;                // Disney SSS
            [Reload("Runtime/Material/SubsurfaceScattering/CombineLighting.shader")]
            public Shader combineLightingPS;

            // General
            [Reload("Runtime/RenderPipeline/RenderPass/MotionVectors/CameraMotionVectors.shader")]
            public Shader cameraMotionVectorsPS;
            [Reload("Runtime/ShaderLibrary/CopyStencilBuffer.shader")]
            public Shader copyStencilBufferPS;
            [Reload("Runtime/ShaderLibrary/CopyDepthBuffer.shader")]
            public Shader copyDepthBufferPS;
            [Reload("Runtime/ShaderLibrary/Blit.shader")]
            public Shader blitPS;

            [Reload("Runtime/ShaderLibrary/DownsampleDepth.shader")]
            public Shader downsampleDepthPS;
            [Reload("Runtime/ShaderLibrary/UpsampleTransparent.shader")]
            public Shader upsampleTransparentPS;

            // Sky
            [Reload("Runtime/Sky/BlitCubemap.shader")]
            public Shader blitCubemapPS;
            [Reload("Runtime/Material/GGXConvolution/BuildProbabilityTables.compute")]
            public ComputeShader buildProbabilityTablesCS;
            [Reload("Runtime/Material/GGXConvolution/ComputeGgxIblSampleData.compute")]
            public ComputeShader computeGgxIblSampleDataCS;
            [Reload("Runtime/Material/GGXConvolution/GGXConvolve.shader")]
            public Shader GGXConvolvePS;
            [Reload("Runtime/Material/Fabric/CharlieConvolve.shader")]
            public Shader charlieConvolvePS;
            [Reload("Runtime/Lighting/AtmosphericScattering/OpaqueAtmosphericScattering.shader")]
            public Shader opaqueAtmosphericScatteringPS;
            [Reload("Runtime/Sky/HDRISky/HDRISky.shader")]
            public Shader hdriSkyPS;
            [Reload("Runtime/Sky/HDRISky/IntegrateHDRISky.shader")]
            public Shader integrateHdriSkyPS;
            [Reload("Runtime/Sky/ProceduralSky/ProceduralSky.shader")]
            public Shader proceduralSkyPS;
            [Reload("Skybox/Cubemap", ReloadAttribute.Package.Builtin)]
            public Shader skyboxCubemapPS;
            [Reload("Runtime/Sky/GradientSky/GradientSky.shader")]
            public Shader gradientSkyPS;
            [Reload("Runtime/Sky/AmbientProbeConvolution.compute")]
            public ComputeShader ambientProbeConvolutionCS;
            public ComputeShader opticalDepthPrecomputationCS;
            public ComputeShader groundIrradiancePrecomputationCS;
            public ComputeShader inScatteredRadiancePrecomputationCS;
            public Shader        pbrSkyPS;

            // Material
            [Reload("Runtime/Material/PreIntegratedFGD/PreIntegratedFGD_GGXDisneyDiffuse.shader")]
            public Shader preIntegratedFGD_GGXDisneyDiffusePS;
            [Reload("Runtime/Material/PreIntegratedFGD/PreIntegratedFGD_CharlieFabricLambert.shader")]
            public Shader preIntegratedFGD_CharlieFabricLambertPS;
            [Reload("Runtime/Material/AxF/PreIntegratedFGD_Ward.shader")]
            public Shader preIntegratedFGD_WardPS;
            [Reload("Runtime/Material/AxF/PreIntegratedFGD_CookTorrance.shader")]
            public Shader preIntegratedFGD_CookTorrancePS;

            // Utilities / Core
            [Reload("Runtime/Core/CoreResources/EncodeBC6H.compute")]
            public ComputeShader encodeBC6HCS;
            [Reload("Runtime/Core/CoreResources/CubeToPano.shader")]
            public Shader cubeToPanoPS;
            [Reload("Runtime/Core/CoreResources/BlitCubeTextureFace.shader")]
            public Shader blitCubeTextureFacePS;
            [Reload("Runtime/Material/LTCAreaLight/FilterAreaLightCookies.shader")]
            public Shader filterAreaLightCookiesPS;

            // Shadow            
            [Reload("Runtime/Lighting/Shadow/ShadowClear.shader")]
            public Shader shadowClearPS;
            [Reload("Runtime/Lighting/Shadow/EVSMBlur.compute")]
            public ComputeShader evsmBlurCS;
            [Reload("Runtime/Lighting/Shadow/DebugDisplayHDShadowMap.shader")]
            public Shader debugHDShadowMapPS;
            [Reload("Runtime/Lighting/Shadow/MomentShadows.compute")]
            public ComputeShader momentShadowsCS;

            // Decal
            [Reload("Runtime/Material/Decal/DecalNormalBuffer.shader")]
            public Shader decalNormalBufferPS;

            // Ambient occlusion
            [Reload("Runtime/Lighting/ScreenSpaceLighting/AmbientOcclusionDownsample1.compute")]
            public ComputeShader aoDownsample1CS;
            [Reload("Runtime/Lighting/ScreenSpaceLighting/AmbientOcclusionDownsample2.compute")]
            public ComputeShader aoDownsample2CS;
            [Reload("Runtime/Lighting/ScreenSpaceLighting/AmbientOcclusionRender.compute")]
            public ComputeShader aoRenderCS;
            [Reload("Runtime/Lighting/ScreenSpaceLighting/AmbientOcclusionUpsample.compute")]
            public ComputeShader aoUpsampleCS;
            [Reload("Runtime/RenderPipeline/RenderPass/MSAA/AmbientOcclusionResolve.shader")]
            public Shader aoResolvePS;

            // MSAA Shaders
            [Reload("Runtime/RenderPipeline/RenderPass/MSAA/DepthValues.shader")]
            public Shader depthValuesPS;
            [Reload("Runtime/RenderPipeline/RenderPass/MSAA/ColorResolve.shader")]
            public Shader colorResolvePS;

            // Post-processing
            [Reload("Runtime/PostProcessing/Shaders/NaNKiller.compute")]
            public ComputeShader nanKillerCS;
            [Reload("Runtime/PostProcessing/Shaders/Exposure.compute")]
            public ComputeShader exposureCS;
            [Reload("Runtime/PostProcessing/Shaders/UberPost.compute")]
            public ComputeShader uberPostCS;
            [Reload("Runtime/PostProcessing/Shaders/LutBuilder3D.compute")]
            public ComputeShader lutBuilder3DCS;
            [Reload("Runtime/PostProcessing/Shaders/TemporalAntialiasing.compute")]
            public ComputeShader temporalAntialiasingCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldKernel.compute")]
            public ComputeShader depthOfFieldKernelCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldCoC.compute")]
            public ComputeShader depthOfFieldCoCCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldCoCReproject.compute")]
            public ComputeShader depthOfFieldCoCReprojectCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldCoCDilate.compute")]
            public ComputeShader depthOfFieldDilateCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldMip.compute")]
            public ComputeShader depthOfFieldMipCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldMipSafe.compute")]
            public ComputeShader depthOfFieldMipSafeCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldPrefilter.compute")]
            public ComputeShader depthOfFieldPrefilterCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldTileMax.compute")]
            public ComputeShader depthOfFieldTileMaxCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldGather.compute")]
            public ComputeShader depthOfFieldGatherCS;
            [Reload("Runtime/PostProcessing/Shaders/DepthOfFieldCombine.compute")]
            public ComputeShader depthOfFieldCombineCS;
            [Reload("Runtime/PostProcessing/Shaders/PaniniProjection.compute")]
            public ComputeShader paniniProjectionCS;
            [Reload("Runtime/PostProcessing/Shaders/MotionBlurMotionVecPrep.compute")]
            public ComputeShader motionBlurMotionVecPrepCS;
            [Reload("Runtime/PostProcessing/Shaders/MotionBlurTilePass.compute")]
            public ComputeShader motionBlurTileGenCS;
            [Reload("Runtime/PostProcessing/Shaders/MotionBlur.compute")]
            public ComputeShader motionBlurCS;
            [Reload("Runtime/PostProcessing/Shaders/BloomPrefilter.compute")]
            public ComputeShader bloomPrefilterCS;
            [Reload("Runtime/PostProcessing/Shaders/BloomBlur.compute")]
            public ComputeShader bloomBlurCS;
            [Reload("Runtime/PostProcessing/Shaders/BloomUpsample.compute")]
            public ComputeShader bloomUpsampleCS;
            [Reload("Runtime/PostProcessing/Shaders/FXAA.compute")]
            public ComputeShader FXAACS;
            [Reload("Runtime/PostProcessing/Shaders/FinalPass.shader")]
            public Shader finalPassPS;
            [Reload("Runtime/PostProcessing/Shaders/ClearBlack.shader")]
            public Shader clearBlackPS;
            [Reload("Runtime/PostProcessing/Shaders/SubpixelMorphologicalAntialiasing.shader")]
            public Shader SMAAPS;

            // Iterator to retrieve all compute shaders in reflection so we don't have to keep a list of
            // used compute shaders up to date (prefer editor-only usage)
            public IEnumerable<ComputeShader> GetAllComputeShaders()
            {
                var fields = typeof(ShaderResources).GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    if (field.GetValue(this) is ComputeShader computeShader)
                        yield return computeShader;
                }
            }
        }

        [Serializable, ReloadGroup]
        public sealed class MaterialResources
        {
            // Defaults
            [Reload("Runtime/RenderPipelineResources/Material/DefaultHDMaterial.mat")]
            public Material defaultDiffuseMat;
            [Reload("Runtime/RenderPipelineResources/Material/DefaultHDMirrorMaterial.mat")]
            public Material defaultMirrorMat;
            [Reload("Runtime/RenderPipelineResources/Material/DefaultHDDecalMaterial.mat")]
            public Material defaultDecalMat;
            [Reload("Runtime/RenderPipelineResources/Material/DefaultHDTerrainMaterial.mat")]
            public Material defaultTerrainMat;
        }

        [Serializable, ReloadGroup]
        public sealed class TextureResources
        {
            // Debug
            [Reload("Runtime/RenderPipelineResources/Texture/DebugFont.tga")]
            public Texture2D debugFontTex;
            [Reload("Runtime/Debug/ColorGradient.png")]
            public Texture2D colorGradient;
            [Reload("Runtime/RenderPipelineResources/Texture/Matcap/DefaultMatcap.png")]
            public Texture2D matcapTex;

            // Pre-baked noise
            [Reload("Runtime/RenderPipelineResources/Texture/BlueNoise16/L/LDR_LLL1_{0}.png", 0, 32)]
            public Texture2D[] blueNoise16LTex;
            [Reload("Runtime/RenderPipelineResources/Texture/BlueNoise16/RGB/LDR_RGB1_{0}.png", 0, 32)]
            public Texture2D[] blueNoise16RGBTex;
            [Reload("Runtime/RenderPipelineResources/Texture/CoherentNoise/OwenScrambledNoise.png")]
            public Texture2D owenScrambledTex;
            [Reload("Runtime/RenderPipelineResources/Texture/CoherentNoise/ScrambleNoise.png")]
            public Texture2D scramblingTex;

            // Post-processing
            [Reload(new[]
            {
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Thin01.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Thin02.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium01.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium02.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium03.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium04.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium05.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Medium06.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Large01.png",
                "Runtime/RenderPipelineResources/Texture/FilmGrain/Large02.png"
            })]
            public Texture2D[] filmGrainTex;
            [Reload("Runtime/RenderPipelineResources/Texture/SMAA/SearchTex.tga")]
            public Texture2D   SMAASearchTex;
            [Reload("Runtime/RenderPipelineResources/Texture/SMAA/AreaTex.tga")]
            public Texture2D   SMAAAreaTex;
        }

        [Serializable, ReloadGroup]
        public sealed class ShaderGraphResources
        {
        }

        [Serializable, ReloadGroup]
        public sealed class AssetResources
        {
            [Reload("Runtime/RenderPipelineResources/defaultDiffusionProfile.asset")]
            public DiffusionProfileSettings defaultDiffusionProfile;
        }

        public ShaderResources shaders;
        public MaterialResources materials;
        public TextureResources textures;
        public ShaderGraphResources shaderGraphs;
        public AssetResources assets;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(RenderPipelineResources))]
    class RenderPipelineResourcesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Add a "Reload All" button in inspector when we are in developer's mode
            if (UnityEditor.EditorPrefs.GetBool("DeveloperMode")
                && GUILayout.Button("Reload All"))
            {
<<<<<<< HEAD
                // Defaults
                defaultPS = Load<Shader>(HDRenderPipelinePath + "Material/Lit/Lit.shader"),

                // Debug
                debugDisplayLatlongPS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugDisplayLatlong.Shader"),
                debugViewMaterialGBufferPS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugViewMaterialGBuffer.Shader"),
                debugViewTilesPS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugViewTiles.Shader"),
                debugFullScreenPS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugFullScreen.Shader"),
                debugColorPickerPS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugColorPicker.Shader"),
                debugLightVolumePS = Load<Shader>(HDRenderPipelinePath + "Debug/DebugLightVolumes.Shader"),
                debugLightVolumeCS = Load<ComputeShader>(HDRenderPipelinePath + "Debug/DebugLightVolumes.compute"),
                // Lighting
                deferredPS = Load<Shader>(HDRenderPipelinePath + "Lighting/Deferred.Shader"),
                colorPyramidCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/ColorPyramid.compute"),
                colorPyramidPS = Load<Shader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/ColorPyramidPS.Shader"),
                depthPyramidCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/DepthPyramid.compute"),
                copyChannelCS = Load<ComputeShader>(CorePath + "CoreResources/GPUCopy.compute"),
                applyDistortionCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/Distortion/ApplyDistorsion.compute"),
                screenSpaceReflectionsCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/ScreenSpaceLighting/ScreenSpaceReflections.compute"),

                // Lighting tile pass
                clearDispatchIndirectCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/cleardispatchindirect.compute"),
                buildDispatchIndirectCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/builddispatchindirect.compute"),
                buildScreenAABBCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/scrbound.compute"),
                buildPerTileLightListCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/lightlistbuild.compute"),
                buildPerBigTileLightListCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/lightlistbuild-bigtile.compute"),
                buildPerVoxelLightListCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/lightlistbuild-clustered.compute"),
                buildMaterialFlagsCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/materialflags.compute"),
                deferredCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/LightLoop/Deferred.compute"),

                screenSpaceShadowCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/Shadow/ScreenSpaceShadow.compute"),
                volumeVoxelizationCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/VolumetricLighting/VolumeVoxelization.compute"),
                volumetricLightingCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/VolumetricLighting/VolumetricLighting.compute"),

                deferredTilePS = Load<Shader>(HDRenderPipelinePath + "Lighting/LightLoop/DeferredTile.shader"),

                subsurfaceScatteringCS = Load<ComputeShader>(HDRenderPipelinePath + "Material/SubsurfaceScattering/SubsurfaceScattering.compute"),
                combineLightingPS = Load<Shader>(HDRenderPipelinePath + "Material/SubsurfaceScattering/CombineLighting.shader"),
                
                // General
                cameraMotionVectorsPS = Load<Shader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/MotionVectors/CameraMotionVectors.shader"),
                copyStencilBufferPS = Load<Shader>(HDRenderPipelinePath + "ShaderLibrary/CopyStencilBuffer.shader"),
                copyDepthBufferPS = Load<Shader>(HDRenderPipelinePath + "ShaderLibrary/CopyDepthBuffer.shader"),
                blitPS = Load<Shader>(HDRenderPipelinePath + "ShaderLibrary/Blit.shader"),

                // Sky
                blitCubemapPS = Load<Shader>(HDRenderPipelinePath + "Sky/BlitCubemap.shader"),
                buildProbabilityTablesCS = Load<ComputeShader>(HDRenderPipelinePath + "Material/GGXConvolution/BuildProbabilityTables.compute"),
                computeGgxIblSampleDataCS = Load<ComputeShader>(HDRenderPipelinePath + "Material/GGXConvolution/ComputeGgxIblSampleData.compute"),
                GGXConvolvePS = Load<Shader>(HDRenderPipelinePath + "Material/GGXConvolution/GGXConvolve.shader"),
                charlieConvolvePS = Load<Shader>(HDRenderPipelinePath + "Material/Fabric/CharlieConvolve.shader"),
                opaqueAtmosphericScatteringPS = Load<Shader>(HDRenderPipelinePath + "Lighting/AtmosphericScattering/OpaqueAtmosphericScattering.shader"),
                hdriSkyPS = Load<Shader>(HDRenderPipelinePath + "Sky/HDRISky/HDRISky.shader"),
                integrateHdriSkyPS = Load<Shader>(HDRenderPipelinePath + "Sky/HDRISky/IntegrateHDRISky.shader"),
                proceduralSkyPS = Load<Shader>(HDRenderPipelinePath + "Sky/ProceduralSky/ProceduralSky.shader"),
                gradientSkyPS = Load<Shader>(HDRenderPipelinePath + "Sky/GradientSky/GradientSky.shader"),
                ambientProbeConvolutionCS = Load<ComputeShader>(HDRenderPipelinePath + "Sky/AmbientProbeConvolution.compute"),
                opticalDepthPrecomputationCS = Load<ComputeShader>(HDRenderPipelinePath + "Sky/PbrSky/OpticalDepthPrecomputation.compute"),
                groundIrradiancePrecomputationCS = Load<ComputeShader>(HDRenderPipelinePath + "Sky/PbrSky/GroundIrradiancePrecomputation.compute"),
                inScatteredRadiancePrecomputationCS = Load<ComputeShader>(HDRenderPipelinePath + "Sky/PbrSky/InScatteredRadiancePrecomputation.compute"),
                pbrSkyPS = Load<Shader>(HDRenderPipelinePath + "Sky/PbrSky/PbrSky.shader"),

                // Skybox/Cubemap is a builtin shader, must use Shader.Find to access it. It is fine because we are in the editor
                skyboxCubemapPS = Shader.Find("Skybox/Cubemap"),

                // Material
                preIntegratedFGD_GGXDisneyDiffusePS = Load<Shader>(HDRenderPipelinePath + "Material/PreIntegratedFGD/PreIntegratedFGD_GGXDisneyDiffuse.shader"),
                preIntegratedFGD_CharlieFabricLambertPS = Load<Shader>(HDRenderPipelinePath + "Material/PreIntegratedFGD/PreIntegratedFGD_CharlieFabricLambert.shader"),
                preIntegratedFGD_CookTorrancePS = Load<Shader>(HDRenderPipelinePath + "Material/AxF/PreIntegratedFGD_CookTorrance.shader"),
                preIntegratedFGD_WardPS = Load<Shader>(HDRenderPipelinePath + "Material/AxF/PreIntegratedFGD_Ward.shader"),

                // Utilities / Core
                encodeBC6HCS = Load<ComputeShader>(CorePath + "CoreResources/EncodeBC6H.compute"),
                cubeToPanoPS = Load<Shader>(CorePath + "CoreResources/CubeToPano.shader"),
                blitCubeTextureFacePS = Load<Shader>(CorePath + "CoreResources/BlitCubeTextureFace.shader"),
                filterAreaLightCookiesPS = Load<Shader>(CorePath + "CoreResources/FilterAreaLightCookies.shader"),

                // Shadow
                shadowClearPS = Load<Shader>(HDRenderPipelinePath + "Lighting/Shadow/ShadowClear.shader"),
                shadowBlurMomentsCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/Shadow/ShadowBlurMoments.compute"),
                debugHDShadowMapPS = Load<Shader>(HDRenderPipelinePath + "Lighting/Shadow/DebugDisplayHDShadowMap.shader"),
                momentShadowsCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/Shadow/MomentShadows.compute"),

                // Decal
                decalNormalBufferPS = Load<Shader>(HDRenderPipelinePath + "Material/Decal/DecalNormalBuffer.shader"),
                
                // Ambient occlusion
                aoDownsample1CS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/ScreenSpaceLighting/AmbientOcclusionDownsample1.compute"),
                aoDownsample2CS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/ScreenSpaceLighting/AmbientOcclusionDownsample2.compute"),
                aoRenderCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/ScreenSpaceLighting/AmbientOcclusionRender.compute"),
                aoUpsampleCS = Load<ComputeShader>(HDRenderPipelinePath + "Lighting/ScreenSpaceLighting/AmbientOcclusionUpsample.compute"),

                // MSAA
                depthValuesPS = Load<Shader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/MSAA/DepthValues.shader"),
                colorResolvePS = Load<Shader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/MSAA/ColorResolve.shader"),
                aoResolvePS = Load<Shader>(HDRenderPipelinePath + "RenderPipeline/RenderPass/MSAA/AmbientOcclusionResolve.shader"),

                // Post-processing
                exposureCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/Exposure.compute"),
                uberPostCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/UberPost.compute"),
                lutBuilder3DCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/LutBuilder3D.compute"),
                temporalAntialiasingCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/TemporalAntialiasing.compute"),
                depthOfFieldKernelCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldKernel.compute"),
                depthOfFieldCoCCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldCoC.compute"),
                depthOfFieldCoCReprojectCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldCoCReproject.compute"),
                depthOfFieldDilateCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldCoCDilate.compute"),
                depthOfFieldMipCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldMip.compute"),
                depthOfFieldMipSafeCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldMipSafe.compute"),
                depthOfFieldPrefilterCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldPrefilter.compute"),
                depthOfFieldTileMaxCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldTileMax.compute"),
                depthOfFieldGatherCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldGather.compute"),
                depthOfFieldCombineCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/DepthOfFieldCombine.compute"),
                motionBlurTileGenCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/MotionBlurTilePass.compute"),
                motionBlurCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/MotionBlur.compute"),
                motionBlurVelocityPrepCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/MotionBlurVelocityPrep.compute"),
                paniniProjectionCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/PaniniProjection.compute"),
                bloomPrefilterCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/BloomPrefilter.compute"),
                bloomBlurCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/BloomBlur.compute"),
                bloomUpsampleCS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/BloomUpsample.compute"),
                FXAACS = Load<ComputeShader>(HDRenderPipelinePath + "PostProcessing/Shaders/FXAA.compute"),
                finalPassPS = Load<Shader>(HDRenderPipelinePath + "PostProcessing/Shaders/FinalPass.shader"),

            
#if ENABLE_RAYTRACING
                ,
                aoRaytracing = Load<RaytracingShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/RaytracingAmbientOcclusion.raytrace"),
                reflectionRaytracing = Load<RaytracingShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/RaytracingReflections.raytrace"),
                shadowsRaytracing = Load<RaytracingShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/RaytracingAreaShadows.raytrace"),
                areaBillateralFilterCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/AreaBilateralShadow.compute"),
                reflectionBilateralFilterCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/GaussianBilateral.compute"),
                lightClusterBuildCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/RaytracingLightCluster.compute"),
                lightClusterDebugCS = Load<ComputeShader>(HDRenderPipelinePath + "RenderPipeline/Raytracing/Shaders/DebugLightCluster.compute")
#endif
        };

            // Materials
            materials = new MaterialResources
            {
            };

            // Textures
            textures = new TextureResources
            {
                // Debug
                debugFontTex = Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/DebugFont.tga"),
                colorGradient = Load<Texture2D>(HDRenderPipelinePath + "Debug/ColorGradient.png"),

                filmGrainTex = new[]
                {
                    // These need to stay in this specific order!
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Thin01.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Thin02.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium01.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium02.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium03.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium04.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium05.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Medium06.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Large01.png"),
                    Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/FilmGrain/Large02.png")
                },

                blueNoise16LTex = new Texture2D[32],
                blueNoise16RGBTex = new Texture2D[32],
                coherentRGNoise128 = new Texture2D[16]
            };

            // ShaderGraphs
            shaderGraphs = new ShaderGraphResources
            {
            };

            // Fill-in blue noise textures
            for (int i = 0; i < 32; i++)
            {
                textures.blueNoise16LTex[i] = Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/BlueNoise16/L/LDR_LLL1_" + i + ".png");
                textures.blueNoise16RGBTex[i] = Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/BlueNoise16/RGB/LDR_RGB1_" + i + ".png");
            }

            // Fill-in coherent noise textures
            for (int i = 0; i < 8; i++)
            {
                textures.coherentRGNoise128[i] = Load<Texture2D>(HDRenderPipelinePath + "RenderPipelineResources/Texture/CoherentNoise128/sample_" + i + "_xy.bmp");
=======
                var resources = target as RenderPipelineResources;
                resources.materials = null;
                resources.textures = null;
                resources.shaders = null;
                resources.shaderGraphs = null;
                ResourceReloader.ReloadAllNullIn(target, HDUtils.GetHDRenderPipelinePath());
>>>>>>> master
            }
        }
    }
#endif
}
