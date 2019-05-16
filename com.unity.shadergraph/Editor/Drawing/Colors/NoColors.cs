using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    class NoColors : IColorProvider
    {
        public static string NoColorTitle = "<None>";
        public string Title => NoColorTitle;
        public bool AllowCustom => false;

        public bool ProvideColorForNode(AbstractMaterialNode node, ref Color color)
        {
            return false;
        }

        public bool ApplyClassForNodeToElement(AbstractMaterialNode node, VisualElement el)
        {
            return true;
        }
    }
}
