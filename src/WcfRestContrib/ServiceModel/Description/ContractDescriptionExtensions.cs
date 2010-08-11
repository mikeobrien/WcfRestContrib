using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using WcfRestContrib.ServiceModel.Configuration;

namespace WcfRestContrib.ServiceModel.Description
{
    public static class ContractDescriptionExtensions
    {
        public static void LoadContractBehaviors(
            this ContractDescription contract,
            string behaviorConfiguration)
        {
            if (string.IsNullOrEmpty(behaviorConfiguration)) 
                throw new ArgumentException("Behavior configuration not specified.");

            ServiceBehaviorElement serviceBehaviors = 
                ConfigurationManager.GetServiceBehaviorElement(behaviorConfiguration);

            if (serviceBehaviors != null)
            {
                foreach (BehaviorExtensionElement behaviorExtension in serviceBehaviors)
                {
                    object extension = behaviorExtension.CreateBehavior();
                    if (extension != null)
                    {
                        Type extensionType = extension.GetType();
                        if (typeof(IContractBehavior).IsAssignableFrom(extensionType))
                        {
                            if (contract.Behaviors.Contains(extensionType))
                            {
                                contract.Behaviors.Remove(extensionType);
                            }
                            contract.Behaviors.Add((IContractBehavior)extension);
                        }
                    }
                }
            }
        }

        public static TBehavior FindBehavior<TBehavior, TAttribute>(
            this ContractDescription contract,
            Func<TAttribute, TBehavior> convert) 
            where TBehavior : class where TAttribute : class
        {
            TBehavior behavior =
                contract.Behaviors.Find<TBehavior>();

            if (behavior == null)
            {
                TAttribute attribute =
                    contract.Behaviors.
                    Find<TAttribute>();
                if (attribute != null) behavior = convert(attribute);
            }
            return behavior;
        }

        public static T GetAttribute<T>(this ContractDescription contract) where T:Attribute
        {
            object[] attributes = contract.ContractType.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0)
                return (T)attributes[0];
            else
                return null;
        }

        public static List<T> GetAttributes<T>(this ContractDescription contract) where T:Attribute
        {
            List<T> attributes = new List<T>();
            object[] results = contract.ContractType.GetCustomAttributes(typeof(T), true);

            foreach (object result in results)
                attributes.Add((T)result);

            return attributes;
        }
    }
}
