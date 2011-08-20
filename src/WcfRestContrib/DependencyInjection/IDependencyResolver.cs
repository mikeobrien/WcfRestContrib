using System;

namespace WcfRestContrib.DependencyInjection
{
    public interface IDependencyResolver
    {
        object GetInfrastructureService(Type serviceType);

        object CreateOperationContainer();
        object GetOperationService(object container, Type serviceType);
        void OperationError(object container, Exception exception);
        void ReleaseOperationContainer(object container);
    }
}
