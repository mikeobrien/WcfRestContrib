using System;
using System.Diagnostics;
using WcfRestContrib.DependencyInjection;

namespace NielsBohrLibrary.Runtime
{
    public class DependencyResolver : IDependencyResolver
    {
        public object GetInfrastructureService(Type serviceType)
        {
            // Insert your favorite DI tool here...
            Debug.WriteLine("GetInfrastructureService({0})", serviceType);
            return CreateObject(serviceType);
        }

        public object CreateOperationContainer()
        {
            // Return a container that will service the operation
            var container = new object();
            Debug.WriteLine("{0} CreateOperationContainer()", container.GetHashCode());
            return container;
        }

        public object GetOperationService(object container, Type serviceType)
        {
            // Insert your favorite DI tool here...
            Debug.WriteLine("GetOperationService({0}, {1})", container.GetHashCode(), serviceType);
            return CreateObject(serviceType);
        }

        public void OperationError(object container, Exception exception)
        {
            // Handle errors yo
            Debug.WriteLine("OperationError({0}, {1}...)", container.GetHashCode(), exception.Message.Substring(0, 20));
        }

        public void ReleaseOperationContainer(object container)
        {
            Debug.WriteLine("ReleaseOperationContainer({0})", container.GetHashCode());
            // Release your operation container
        }

        private static object CreateObject(Type type)
        {
            if (type.IsAbstract) return null;
            return Activator.CreateInstance(type);
        }
    }
}