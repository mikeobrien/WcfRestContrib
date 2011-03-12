using System;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebErrorHandlerConfigurationBehavior : IServiceBehavior
    {
        private readonly Type _exceptionDataContract;

        public WebErrorHandlerConfigurationBehavior(
            Type logHandler,
            string unhandledErrorMessage,
            bool returnRawException,
            Type exceptionDataContract)
        {
            LogHandler = logHandler;
            UnhandledErrorMessage = unhandledErrorMessage;
            ReturnRawException = returnRawException;
            _exceptionDataContract = exceptionDataContract;
        }

        public Type LogHandler { get; set; }
        public Type ExceptionDataContract { get; set; }
        public string UnhandledErrorMessage { get; set; }
        public bool ReturnRawException { get; set; }
        public bool HasExceptionDataContract { get { return _exceptionDataContract != null; } }

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

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
    }
}
