using System.Linq;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;

namespace WcfRestContrib.ServiceModel.Configuration
{
    public static class CustomBindingElementExtensions
    {
        public static void RemoveDuplicateBindingExtensions(this CustomBindingElement bindingElement, CustomBinding binding)
        {
            foreach (var element in
                bindingElement.Select(elementExtension => binding.Elements.
                    FirstOrDefault(e => e.GetType() == elementExtension.BindingElementType)).
                    Where(element => element != null))
            {
                binding.Elements.Remove(element);
            }
        }
    }
}
