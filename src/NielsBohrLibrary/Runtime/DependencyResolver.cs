using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WcfRestContrib.DependencyInjection;

namespace NielsBohrLibrary.Runtime
{
    public class DependencyResolver : IDependencyResolver
    {
        public object GetInfrastructureService(Type serviceType)
        {
            // Insert your favorite DI tool here...
            Debug.WriteLine("GetInfrastructureService({0})", serviceType);
            return Activator.CreateInstance(serviceType);
        }

        public object CreateOperationContainer()
        {
            // Return a container that will service the operation
            Debug.WriteLine("CreateOperationContainer()");
            return new object();
        }

        public object GetOperationService(object container, Type serviceType)
        {
            // Insert your favorite DI tool here...
            Debug.WriteLine("GetOperationService({0}, {1})", container.GetHashCode(), serviceType);
            return Activator.CreateInstance(serviceType);
        }

        public void ReleaseOperationContainer(object container)
        {
            Debug.WriteLine("ReleaseOperationContainer({0})", container.GetHashCode());
            // Release your operation container
        }
    }
}