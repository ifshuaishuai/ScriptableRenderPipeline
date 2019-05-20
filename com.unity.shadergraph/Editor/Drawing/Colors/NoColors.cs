using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    internal class NoColors : IColorProvider
    {
        public static string NoColorTitle = "<None>";
        public string GetTitle() => NoColorTitle;

        public bool AllowCustom() => false;

        public void ApplyColor(IShaderNodeView nodeView)
        {
        }

        public void ClearColor(IShaderNodeView nodeView)
        {
        }
    }
}
