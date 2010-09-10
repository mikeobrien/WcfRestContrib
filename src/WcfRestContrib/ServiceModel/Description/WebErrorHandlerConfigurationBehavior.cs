using System;
using System.ServiceModel.Description;
using WcfRestContrib.Diagnostics;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebErrorHandlerConfigurationBehavior : IServiceBehavior
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly Type _exceptionDataContract;

        // ────────────────────────── Constructors ──────────────────────────

        public WebErrorHandlerConfigurationBehavior(
            Type logHandler,
            string unhandledErrorMessage,
            bool returnRawException,
            Type exceptionDataContract)
        {
            if (logHandler != null) 
                LogHandler = (IWebLogHandler)Activator.CreateInstance(logHandler);
            UnhandledErrorMessage = unhandledErrorMessage;
            ReturnRawException = returnRawException;
            _exceptionDataContract = exceptionDataContract;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public IWebLogHandler LogHandler { get; set; }
        public string UnhandledErrorMessage { get; set; }
        public bool ReturnRawException { get; set; }
        public bool HasExceptionDataContract
        { get { return _exceptionDataContract != null; } }

        public IWebExceptionDataContract CreateExceptionDataContract()
        {
            try
            {
                return (IWebExceptionDataContract)Activator.CreateInstance(_exceptionDataContract);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to create web exception data contract {0}: {1}", _exceptionDataContract.Name, e.Message), e);
            }
        }

        // ────────────────────────── IServiceBehavior Members ──────────────────────────

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
    }
}
