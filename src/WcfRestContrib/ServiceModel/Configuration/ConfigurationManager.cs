using System.Linq;
using System.ServiceModel.Configuration;

namespace WcfRestContrib.ServiceModel.Configuration
{
    public static class ConfigurationManager
    {
        private const string ServiceModelElement = "system.serviceModel/";
        private const string ServicesElement = ServiceModelElement + "services";
        private const string BehaviorsElement = ServiceModelElement + "behaviors";
        private const string BindingsElement = ServiceModelElement + "bindings";

        public static ServiceBehaviorElement GetServiceBehaviorElement(string behaviorConfiguration)
        {
            var behaviorsSection = 
                (BehaviorsSection)System.Configuration.ConfigurationManager.GetSection(BehaviorsElement);
            return behaviorsSection.ServiceBehaviors.Cast<ServiceBehaviorElement>().FirstOrDefault(behavior => behavior.Name == behaviorConfiguration);
        }

        public static ServicesSection GetServiceSection()
        {
            return (ServicesSection)System.Configuration.ConfigurationManager.GetSection(ServicesElement);
        }

        public static ServiceElement GetServiceElement(string serviceName)
        {
            var servicesSection = GetServiceSection();
            var services = servicesSection.Services;
            return services.Cast<ServiceElement>().FirstOrDefault(element => element.Name == serviceName);
        }

        public static BindingsSection GetBindingsSection()
        {
            return (BindingsSection)System.Configuration.ConfigurationManager.GetSection(BindingsElement);
        }
    }
}
