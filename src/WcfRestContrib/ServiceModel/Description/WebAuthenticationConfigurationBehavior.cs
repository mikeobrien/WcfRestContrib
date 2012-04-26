using System;
using System.ServiceModel.Description;
using WcfRestContrib.DependencyInjection;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebAuthenticationConfigurationBehavior : IServiceBehavior, IContractBehavior
    {
        public WebAuthenticationConfigurationBehavior(
            Type authenticationHandler,
            Type usernamePasswordValidator,
            bool requireSecureTransport,
            string source)
        {
            AuthenticationHandler = authenticationHandler != null
                ? DependencyResolver.Current.GetInfrastructureService<IWebAuthenticationHandler>(authenticationHandler)
                : DependencyResolver.Current.GetInfrastructureService<IWebAuthenticationHandler>().ThrowIfNull();

            UsernamePasswordValidatorType = usernamePasswordValidator;

            Source = source;
            RequireSecureTransport = requireSecureTransport;
        }

        public IWebAuthenticationHandler AuthenticationHandler { get; private set; }
        public Type UsernamePasswordValidatorType { get; private set; }
        public bool RequireSecureTransport { get; set; }
        public string Source { get; set; }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) { }
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime) { }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
    }
}
