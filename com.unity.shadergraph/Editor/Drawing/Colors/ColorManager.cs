using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Drawing.Colors
{
    // Use this to set colors on your node titles.
    // There are 2 methods of setting colors - direct Color objects via code (such as data saved in the node itself),
    // or setting classes on a VisualElement, allowing the colors themselves to be defined in USS. See notes on
    // ColorProvider for how to use these different methods.
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
                if (!IsValidIndex(value))
                    return;
                
                m_ActiveIndex = value;
            }
        }

        public ColorManager(string activeProvider)
        {
            m_Providers = new List<IColorProvider>();

            if (string.IsNullOrEmpty(activeProvider))
                activeProvider = DefaultProvider;

            foreach (var colorType in TypeCache.GetTypesDerivedFrom<IColorProvider>().Where(t => !t.IsAbstract))
            {
                var provider = (IColorProvider) Activator.CreateInstance(colorType);
                m_Providers.Add(provider);
            }
            
            m_Providers.Sort((p1, p2) => string.Compare(p1.GetTitle(), p2.GetTitle(), StringComparison.InvariantCulture));
            activeIndex = m_Providers.FindIndex(provider => provider.GetTitle() == activeProvider);
        }

        public void SetActiveProvider(int newIndex, IEnumerable<IShaderNodeView> nodeViews)
        {
            if (newIndex == activeIndex || !IsValidIndex(newIndex))
                return;
            
            var oldProvider = curProvider;
            activeIndex = newIndex;

            foreach (var view in nodeViews)
            {
                oldProvider.ClearColor(view);
                curProvider.ApplyColor(view);
            }
        }
        public void UpdateNodeView(IShaderNodeView nodeView)
        {
            curProvider.ApplyColor(nodeView);
        }

        public IEnumerable<string> providerNames
        {
            get => m_Providers.Select(p => p.GetTitle());
        }

        public string activeProviderName
        {
            get => curProvider.GetTitle();
        }

        public bool activeSupportsCustom
        {
            get => curProvider.AllowCustom();
        }

        IColorProvider curProvider => m_Providers[activeIndex];
        
        bool IsValidIndex(int index)
        {
            return index >= 0 && index < m_Providers.Count;
        }
    }
}
