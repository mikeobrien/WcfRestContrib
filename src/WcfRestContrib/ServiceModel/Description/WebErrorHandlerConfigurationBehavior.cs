using System;
using System.ServiceModel.Description;
using WcfRestContrib.Diagnostics;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.DependencyInjection;

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
            LogHandler = logHandler != null
                            ? DependencyResolver.Current.GetInfrastructureService<IWebLogHandler>(logHandler)
                            : DependencyResolver.Current.GetInfrastructureService<IWebLogHandler>();
            
            UnhandledErrorMessage = unhandledErrorMessage;
            ReturnRawException = returnRawException;
            _exceptionDataContract = exceptionDataContract;
        }

        public IWebLogHandler LogHandler { get; set; }
        public string UnhandledErrorMessage { get; set; }
        public bool ReturnRawException { get; set; }

        public IWebExceptionDataContract CreateExceptionDataContract()
        {
            return _exceptionDataContract != null
                        ? DependencyResolver.Current.GetInfrastructureService<IWebExceptionDataContract>(_exceptionDataContract)
                        : DependencyResolver.Current.GetInfrastructureService<IWebExceptionDataContract>();
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
    }
}
