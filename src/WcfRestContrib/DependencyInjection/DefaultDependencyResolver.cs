using System;
using System.Collections.Generic;
using System.Linq;

namespace WcfRestContrib.DependencyInjection
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        public static IDependencyResolver Instance = new DefaultDependencyResolver();

        public object GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }
    }
}
