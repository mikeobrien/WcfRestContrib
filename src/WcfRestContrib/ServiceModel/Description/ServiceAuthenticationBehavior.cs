using System;
using System.IdentityModel.Selectors;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Description
{
    public class ServiceAuthenticationBehavior : IServiceBehavior, IContractBehavior 
    {
        public class ServiceAuthenticationConfigurationMissingException : Exception
        {
            public ServiceAuthenticationConfigurationMissingException() : 
                base("ServiceAuthenticationConfigurationBehavior not applied to contract or service. This behavior is required to configure service authentication.") {}
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            var behavior = 
                serviceHostBase.Description.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                        WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ServiceAuthenticationConfigurationMissingException();

            var authenticationHandler = ServiceLocator.Current.Create<IWebAuthenticationHandler>(behavior.AuthenticationHandler);
            var usernamePasswordValidator = ServiceLocator.Current.Create<UserNamePasswordValidator>(behavior.UsernamePasswordValidator);

            foreach (ChannelDispatcher dispatcher in 
                serviceHostBase.ChannelDispatchers)
                foreach (var endpoint in dispatcher.Endpoints)
                    endpoint.DispatchRuntime.MessageInspectors.Add(
                        new ServiceAuthenticationInspector(
                            authenticationHandler,
                            usernamePasswordValidator,
                            behavior.RequireSecureTransport,
                            behavior.Source));
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            var behavior = 
                dispatchRuntime.ChannelDispatcher.Host.Description.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                        WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);
            
            if (behavior == null)
                behavior = contractDescription.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                        WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ServiceAuthenticationConfigurationMissingException();

            var authenticationHandler = ServiceLocator.Current.Create<IWebAuthenticationHandler>(behavior.AuthenticationHandler);
            var usernamePasswordValidator = ServiceLocator.Current.Create<UserNamePasswordValidator>(behavior.UsernamePasswordValidator);

            foreach (var endpointDispatcher in dispatchRuntime.ChannelDispatcher.Endpoints)
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                    new ServiceAuthenticationInspector(
                        authenticationHandler,
                        usernamePasswordValidator,
                        behavior.RequireSecureTransport,
                        behavior.Source));
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }
    }
}
