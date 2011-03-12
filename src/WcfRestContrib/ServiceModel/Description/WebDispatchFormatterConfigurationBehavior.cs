using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebDispatchFormatterConfigurationBehavior : IServiceBehavior, IContractBehavior 
    {
        public WebDispatchFormatterConfigurationBehavior(
            Dictionary<string, Type> formatters,
            string defaultMimeType)
        {
            FormatterFactory = new WebFormatterFactory(formatters, defaultMimeType);
        }

        public WebFormatterFactory FormatterFactory { get; set; }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) { }
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime) { }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
    }
}
