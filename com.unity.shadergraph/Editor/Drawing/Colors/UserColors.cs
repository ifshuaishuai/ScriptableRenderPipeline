using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    class UserColors : ColorProviderFromCode 
    {
        string m_Title = "User Defined";
        public override string GetTitle() => m_Title;

        public override bool AllowCustom() => true;

        protected override bool GetColorFromNode(AbstractMaterialNode node, out Color color)
        {
            color = Color.black;
            return node.TryGetColor(m_Title, ref color);
        }
    }
}
