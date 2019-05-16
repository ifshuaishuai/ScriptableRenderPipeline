using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    class UserColors : IColorProvider 
    {
        string m_Title = "User Defined";
        public bool AllowCustom => true;

        public UserColors() {}

        public string Title => m_Title;

        public bool ProvideColorForNode(AbstractMaterialNode node, ref Color color)
        {
            return node.TryGetColor(m_Title, ref color);
        }

        public bool ApplyClassForNodeToElement(AbstractMaterialNode node, VisualElement el)
        {
            return false;
        }
    }
}
