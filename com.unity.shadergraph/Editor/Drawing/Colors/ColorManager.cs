using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    // Use this to set colors on your node titles.
    // There are 2 methods of setting colors - direct Color objects via code (such as data saved in the node itself),
    // or setting classes on a VisualElement, allowing the colors themselves to be defined in USS. See notes on
    // IColorProvider for how to use these different methods.
    class ColorManager
    {
        public static string StyleFile = "ColorMode"; 
        static string DefaultProvider = NoColors.NoColorTitle;
    
        List<IColorProvider> m_Providers;
        
        int m_ActiveIndex = 0;
        public int activeIndex
        {
            get => m_ActiveIndex;
            set
            {
                if (value < 0 || value >= m_Providers.Count)
                    return;
                
                m_ActiveIndex = value;
            }
        }

        public ColorManager(string activeProvider)
        {
            m_Providers = new List<IColorProvider>();

            if (string.IsNullOrEmpty(activeProvider))
                activeProvider = DefaultProvider;

            foreach (var colorType in TypeCache.GetTypesDerivedFrom<IColorProvider>())
            {
                var provider = (IColorProvider) Activator.CreateInstance(colorType);
                m_Providers.Add(provider);
            }
            
            m_Providers.Sort((p1, p2) => string.Compare(p1.Title, p2.Title, StringComparison.InvariantCulture));
            activeIndex = m_Providers.FindIndex(provider => provider.Title == activeProvider);
        }

        public void UpdateNodeView(IShaderNodeView nodeView)
        {
            var curProvider = m_Providers[m_ActiveIndex];
            nodeView.colorElement.ClearClassList();
            if (curProvider.ApplyClassForNodeToElement(nodeView.node, nodeView.colorElement))
            {
                nodeView.ResetColor();
                return;
            }

            var color = Color.black;
            if (curProvider.ProvideColorForNode(nodeView.node, ref color))
            {
                nodeView.SetColor(color);
            }
            else
            {
                nodeView.ResetColor();
            }
        }

        public IEnumerable<string> providerNames
        {
            get => m_Providers.Select(p => p.Title);
        }

        public string activeProviderName
        {
            get => m_Providers[activeIndex].Title;
        }

        public bool activeSupportsCustom
        {
            get => m_Providers[activeIndex].AllowCustom;
        }
    }
}
