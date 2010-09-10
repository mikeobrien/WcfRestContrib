using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.Reflection;
using WcfRestContrib.Diagnostics;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebErrorHandlerConfigurationAttribute : Attribute, IServiceBehavior
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly WebErrorHandlerConfigurationBehavior _behavior;

        // ────────────────────────── Constructors ──────────────────────────

        public WebErrorHandlerConfigurationAttribute(
            Type logHandler, 
            bool returnRawException)
            : this(logHandler, null, returnRawException, null) { }

        public WebErrorHandlerConfigurationAttribute(
            Type logHandler, 
            string unhandledErrorMessage, 
            bool returnRawException)
        : this(logHandler, unhandledErrorMessage, returnRawException, null) { }

        public WebErrorHandlerConfigurationAttribute(
            string unhandledErrorMessage, 
            bool returnRawException, 
            Type exceptionDataContract)
            : this(null, unhandledErrorMessage, returnRawException, exceptionDataContract) { }

        public WebErrorHandlerConfigurationAttribute(
            Type logHandler, 
            bool returnRawException, 
            Type exceptionDataContract)
            : this(logHandler, null, returnRawException, exceptionDataContract) { }

        public WebErrorHandlerConfigurationAttribute(
            Type logHandler, string unhandledErrorMessage, 
            bool returnRawException, 
            Type exceptionDataContract)
        {
            if (logHandler != null && !logHandler.CastableAs<IWebLogHandler>())
                throw new Exception(string.Format("logHandler {0} must implement IWebLogHandler.", logHandler.Name));

            if (exceptionDataContract!= null && !exceptionDataContract.CastableAs<IWebExceptionDataContract>())
                throw new Exception(string.Format("unhandledErrorMessage {0} must implement IWebExceptionDataContract.", exceptionDataContract.Name));

            _behavior = new WebErrorHandlerConfigurationBehavior(logHandler, unhandledErrorMessage, returnRawException, exceptionDataContract);
        }

        // ────────────────────────── Public Members ──────────────────────────

        public WebErrorHandlerConfigurationBehavior BaseBehavior
        { get { return _behavior; } }

        // ────────────────────────── IServiceBehavior Members ──────────────────────────

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters); }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.ApplyDispatchBehavior(serviceDescription, serviceHostBase); }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.Validate(serviceDescription, serviceHostBase); }
    }
}
