using System;
using System.Collections.Generic;

namespace WcfRestContrib.DependencyInjection
{
    public interface IDependencyResolver
    {
        object GetInfrastructureService(Type serviceType);

        object CreateOperationContainer();
        object GetOperationService(object container, Type serviceType);
        void ReleaseOperationContainer(object container);
    }
}
