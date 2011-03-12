using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Description
{
    public class DependencyInjectionAttribute : Attribute, IServiceBehavior
    {
        readonly DependencyInjectionBehavior _behavior;

        public DependencyInjectionAttribute(Type objectFactory)
        {
            if (!objectFactory.CastableAs<IObjectFactory>())
                throw new Exception("objectFactory must implement IObjectFactory.");

            _behavior = new DependencyInjectionBehavior(objectFactory);
        }

        public DependencyInjectionBehavior BaseBehavior
        { get { return _behavior; } }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters); }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.ApplyDispatchBehavior(serviceDescription, serviceHostBase); }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.Validate(serviceDescription, serviceHostBase); }
    }
}
