using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;
using System.IdentityModel.Selectors;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebAuthenticationConfigurationAttribute : Attribute, IServiceBehavior 
    {
        readonly WebAuthenticationConfigurationBehavior _behavior;

        public WebAuthenticationConfigurationAttribute(
            bool requireSecureTransport,
            string source) : this(null, null, requireSecureTransport, source) { }

        public WebAuthenticationConfigurationAttribute(
            Type authenticationHandler,
            bool requireSecureTransport,
            string source) : this(authenticationHandler, null, requireSecureTransport, source) { }

        public WebAuthenticationConfigurationAttribute(
            Type authenticationHandler,
            Type usernamePasswordValidator,
            bool requireSecureTransport,
            string source)
        {
            if (authenticationHandler != null && !authenticationHandler.CastableAs<IWebAuthenticationHandler>())
                throw new Exception(string.Format("authenticationHandler {0} must implement IWebAuthenticationHandler.", authenticationHandler.Name));

            if (usernamePasswordValidator != null && !usernamePasswordValidator.CastableAs<UserNamePasswordValidator>())
                throw new Exception(string.Format("usernamePasswordValidator {0} must inherit from UserNamePasswordValidator.", usernamePasswordValidator.Name));

             _behavior = new WebAuthenticationConfigurationBehavior(
                authenticationHandler,
                usernamePasswordValidator,
                requireSecureTransport,
                source);
        }

        public WebAuthenticationConfigurationBehavior BaseBehavior
        { get { return _behavior; } }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters); }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.ApplyDispatchBehavior(serviceDescription, serviceHostBase); }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.Validate(serviceDescription, serviceHostBase); }
    }
}
