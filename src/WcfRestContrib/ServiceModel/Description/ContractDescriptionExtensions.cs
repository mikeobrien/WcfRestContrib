using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
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

            var serviceBehaviors = 
                ConfigurationManager.GetServiceBehaviorElement(behaviorConfiguration);

            if (serviceBehaviors == null) return;

            foreach (var behaviorExtension in serviceBehaviors)
            {
                var extension = behaviorExtension.CreateBehavior();
                if (extension == null) continue;

                var extensionType = extension.GetType();
                if (!typeof (IContractBehavior).IsAssignableFrom(extensionType)) continue;

                if (contract.Behaviors.Contains(extensionType))
                {
                    contract.Behaviors.Remove(extensionType);
                }
                contract.Behaviors.Add((IContractBehavior)extension);
            }
        }

        public static TBehavior FindBehavior<TBehavior, TAttribute>(
            this ContractDescription contract,
            Func<TAttribute, TBehavior> convert) 
            where TBehavior : class where TAttribute : class
        {
            var behavior =
                contract.Behaviors.Find<TBehavior>();

            if (behavior == null)
            {
                var attribute =
                    contract.Behaviors.
                    Find<TAttribute>();
                if (attribute != null) behavior = convert(attribute);
            }
            return behavior;
        }

        public static T GetAttribute<T>(this ContractDescription contract) where T:Attribute
        {
            var attributes = contract.ContractType.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0) return (T)attributes[0];
            return null;
        }

        public static List<T> GetAttributes<T>(this ContractDescription contract) where T:Attribute
        {
            var results = contract.ContractType.GetCustomAttributes(typeof(T), true);
            return results.Cast<T>().ToList();
        }
    }
}
