using System;
using System.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class DependencyInjectionInstanceProvider : IInstanceProvider
    {
        private readonly IObjectFactory _objectFactory;
        private readonly Type _serviceType;

        public DependencyInjectionInstanceProvider(IObjectFactory objectFactory, Type serviceType)
        {
            _objectFactory = objectFactory;
            _serviceType = serviceType;
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return GetInstance(instanceContext);
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return _objectFactory.Create<object>(_serviceType);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance) { }
    }
}
