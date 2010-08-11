using System;
using System.ServiceModel.Configuration;
using System.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration
{
    public static class BehaviorExtensionElementExtensions
    {
        public static object CreateBehavior(this BehaviorExtensionElement extensionElement)
        {
            try
            {
                return extensionElement.GetType().
                    GetMethod(
                        "CreateBehavior",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic).
                        Invoke(extensionElement, new object[0] { });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;                
            }
        }
    }
}
