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
                if (OperationContainer.Exists()) DependencyResolver.Current.ReleaseOperationContainer(OperationContainer.GetCurrent()); };
            return DependencyResolver.Current.GetOperationService<object>(OperationContainer.GetCurrent(), _serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }
    }
}
