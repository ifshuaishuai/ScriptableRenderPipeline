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

        public Color? ProvideColorForNode(AbstractMaterialNode node)
        {
            return node.GetColor(m_Title);
        }

        public bool ApplyClassForNodeToElement(AbstractMaterialNode node, VisualElement el)
        {
            return false;
        }
    }
}
