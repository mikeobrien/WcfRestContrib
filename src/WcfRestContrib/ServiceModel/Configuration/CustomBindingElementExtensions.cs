using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;

namespace WcfRestContrib.ServiceModel.Configuration
{
    public static class CustomBindingElementExtensions
    {
        public static void RemoveDuplicateBindingExtensions(this CustomBindingElement bindingElement, CustomBinding binding)
        {
            foreach (BindingElementExtensionElement elementExtension in bindingElement)
            {
                BindingElement element = binding.Elements.FirstOrDefault(
                    e => e.GetType() == elementExtension.BindingElementType);
                if (element != null) binding.Elements.Remove(element);
            }
        }
    }
}
