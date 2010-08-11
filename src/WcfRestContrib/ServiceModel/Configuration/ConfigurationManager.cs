using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace WcfRestContrib.ServiceModel.Configuration
{
    public static class ConfigurationManager
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string SERVICE_MODEL_ELEMENT = "system.serviceModel/";
        private const string SERVICES_ELEMENT = SERVICE_MODEL_ELEMENT + "services";
        private const string BEHAVIORS_ELEMENT = SERVICE_MODEL_ELEMENT + "behaviors";
        private const string BINDINGS_ELEMENT = SERVICE_MODEL_ELEMENT + "bindings";

        // ────────────────────────── Public Members ──────────────────────────

        public static ServiceBehaviorElement GetServiceBehaviorElement(string behaviorConfiguration)
        {
            BehaviorsSection behaviorsSection = 
                (BehaviorsSection)System.Configuration.ConfigurationManager.GetSection(BEHAVIORS_ELEMENT);
            foreach (ServiceBehaviorElement behavior in behaviorsSection.ServiceBehaviors)
            {
                if (behavior.Name == behaviorConfiguration)
                    return behavior;
            }
            return null;
        }

        public static ServicesSection GetServiceSection()
        {
            return (ServicesSection)System.Configuration.ConfigurationManager.GetSection(SERVICES_ELEMENT);
        }

        public static ServiceElement GetServiceElement(string serviceName)
        {
            ServicesSection servicesSection = GetServiceSection();
            ServiceElementCollection services = servicesSection.Services;
            foreach (ServiceElement element in services)
            {
                if (element.Name == serviceName)
                    return element;
            }
            return null;
        }

        public static BindingsSection GetBindingsSection()
        {
            return (BindingsSection)System.Configuration.ConfigurationManager.GetSection(BINDINGS_ELEMENT);
        }
    }
}
