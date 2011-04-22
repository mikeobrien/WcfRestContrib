using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class DependencyInjectionInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public DependencyInjectionInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            OperationContext.Current.OperationCompleted += (s, e) => { 
                if (OperationContainer != null) DependencyResolver.Current.ReleaseOperationContainer(GetOperationContainer()); };
            return DependencyResolver.Current.GetOperationService<object>(GetOperationContainer(), _serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }

        private static object GetOperationContainer()
        {
            var extension = OperationContainer;
            if (extension == null)
            {
                extension = new ContainerExtension(DependencyResolver.Current.CreateOperationContainer());
                OperationContext.Current.Extensions.Add(extension);
            }
            return extension.Container;
        }

        private static ContainerExtension OperationContainer
        {
            get { return OperationContext.Current.Extensions.Find<ContainerExtension>(); }
        }

        public class ContainerExtension : IExtension<OperationContext>
        {
            public ContainerExtension(object container) { Container = container; }
            public object Container { get; private set; }
            public void Attach(OperationContext owner) { }
            public void Detach(OperationContext owner) { }
        }
    }
}
