using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Description
{
    public static class ServiceDescriptionExtensions
    {
        public static TBehavior FindBehavior<TBehavior, TAttribute>(
            this ServiceDescription service,
            Func<TAttribute, TBehavior> convert) 
            where TBehavior : class where TAttribute : class
        {
            var behavior =
                service.Behaviors.Find<TBehavior>();

            if (behavior == null)
            {
                var attribute =
                    service.Behaviors.
                    Find<TAttribute>();
                if (attribute != null) behavior = convert(attribute);
            }
            return behavior;
        }

        public static List<T> GetAttributes<T>(this ServiceDescription service) where T:Attribute
        {
            var results = service.ServiceType.GetCustomAttributes(typeof(T), true);
            return results.Cast<T>().ToList();
        }

        public static T GetAttribute<T>(this ServiceDescription service) where T:Attribute
        {
            var attributes = service.ServiceType.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0) return (T)attributes[0];
            return null;
        }

    }
}
