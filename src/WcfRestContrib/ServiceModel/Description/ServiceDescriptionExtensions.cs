using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using WcfRestContrib.ServiceModel.Configuration;

namespace WcfRestContrib.ServiceModel.Description
{
    public static class ServiceDescriptionExtensions
    {
        public static TBehavior FindBehavior<TBehavior, TAttribute>(
            this ServiceDescription service,
            Func<TAttribute, TBehavior> convert) 
            where TBehavior : class where TAttribute : class
        {
            TBehavior behavior =
                service.Behaviors.Find<TBehavior>();

            if (behavior == null)
            {
                TAttribute attribute =
                    service.Behaviors.
                    Find<TAttribute>();
                if (attribute != null) behavior = convert(attribute);
            }
            return behavior;
        }

        public static List<T> GetAttributes<T>(this ServiceDescription service) where T:Attribute
        {
            List<T> attributes = new List<T>();
            object[] results = service.ServiceType.GetCustomAttributes(typeof(T), true);

            foreach (object result in results)
                attributes.Add((T)result);

            return attributes;
        }

        public static T GetAttribute<T>(this ServiceDescription service) where T:Attribute
        {
            object[] attributes = service.ServiceType.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0)
                return (T)attributes[0];
            else
                return null;
        }

    }
}
