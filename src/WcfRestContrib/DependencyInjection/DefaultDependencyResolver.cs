using System;
using System.Collections.Generic;
using System.Linq;

namespace WcfRestContrib.DependencyInjection
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        public object GetInfrastructureService(Type serviceType)
        {
            return CreateInstance(serviceType);
        }

        public object CreateOperationContainer() { return null; }

        public object GetOperationService(object container, Type serviceType)
        {
            return CreateInstance(serviceType);
        }

        public void ReleaseOperationContainer(object container) { }

        private static object CreateInstance(Type type)
        {
            return type.IsAbstract || type.IsInterface ? null : Activator.CreateInstance(type);
        }
    }
}
