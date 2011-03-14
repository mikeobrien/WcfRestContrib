using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using WcfRestContrib.DependencyInjection;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class DependencyInjectionBehavior : IServiceBehavior
    {
        public DependencyInjectionBehavior(Type type)
        {
            if (ServiceLocator.IsDefault()) ServiceLocator.Current = (IObjectFactory)Activator.CreateInstance(type);
        }

        public void ApplyDispatchBehavior(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceHostBase.ChannelDispatchers.
                                                        OfType<ChannelDispatcher>().
                                                        SelectMany(dispatcher => dispatcher.Endpoints))
                endpoint.DispatchRuntime.InstanceProvider = new DependencyInjectionInstanceProvider(serviceDescription.ServiceType);
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, 
                                         BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}
