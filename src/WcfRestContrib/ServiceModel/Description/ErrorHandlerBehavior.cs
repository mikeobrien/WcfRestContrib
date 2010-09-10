using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using System.Net;
using WcfRestContrib.Net.Http;

namespace WcfRestContrib.ServiceModel.Description
{
    public class ErrorHandlerBehavior : IServiceBehavior, IContractBehavior
    {
        // ────────────────────────── Private Fields ──────────────────────────

        public const string HttpRequestInformationProperty = "HttpRequestInformation";

        private readonly IErrorHandler _errorHandler;

        // ────────────────────────── Constructors ──────────────────────────

        public ErrorHandlerBehavior(Type type,
            string unhandledErrorMessage,
            bool returnRawException)
        {
            _errorHandler =
                (IErrorHandler)Activator.CreateInstance(type);
            UnhandledErrorMessage = unhandledErrorMessage;
            ReturnRawException = returnRawException;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public string UnhandledErrorMessage { get; set; }
        public bool ReturnRawException { get; set; }

        // ────────────────────────── IServiceBehavior Members ──────────────────────────

        public void ApplyDispatchBehavior(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in 
                serviceHostBase.ChannelDispatchers)
            {
                if (!dispatcher.ErrorHandlers.Contains(_errorHandler))
                    dispatcher.ErrorHandlers.Add(_errorHandler);

                foreach (var endpoint in dispatcher.Endpoints)
                    if (endpoint.DispatchRuntime.MessageInspectors.FirstOrDefault(
                        i => i.GetType() == typeof(HttpRequestInformationInspector)) == null)
                        endpoint.DispatchRuntime.MessageInspectors.Add(new HttpRequestInformationInspector());
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }


        // ────────────────────────── IContractBehavior Members ──────────────────────────

        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            DispatchRuntime dispatchRuntime) 
        { 
            if (!dispatchRuntime.ChannelDispatcher.ErrorHandlers.Contains(_errorHandler))
                dispatchRuntime.ChannelDispatcher.ErrorHandlers.Add(_errorHandler);

            foreach (var endpointDispatcher in dispatchRuntime.ChannelDispatcher.Endpoints)
                if (endpointDispatcher.DispatchRuntime.MessageInspectors.FirstOrDefault(
                        i => i.GetType() == typeof(HttpRequestInformationInspector)) == null)
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new HttpRequestInformationInspector());
        }
        
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }

        // ────────────────────────── Private Types ──────────────────────────

        private class HttpRequestInformationInspector : IDispatchMessageInspector   
        {
            // ────────────────────────── IDispatchMessageInspector Members ──────────────────────────

            public object AfterReceiveRequest(ref Message request, 
                IClientChannel channel, InstanceContext instanceContext)
            {
                if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(
                        HttpRequestInformationProperty)) return null;

                var info = new RequestInformation();

                var contentLengthHeader =
                    WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.ContentLength];

                long contentLength;
                if (!string.IsNullOrEmpty(contentLengthHeader))
                    long.TryParse(contentLengthHeader, out contentLength);
                else
                    contentLength = -1;

                info.ContentLength = contentLength;
                info.Uri = OperationContext.Current.IncomingMessageHeaders.To;
                info.Method = WebOperationContext.Current.IncomingRequest.Method;
                info.ContentType = WebOperationContext.Current.IncomingRequest.ContentType;
                info.Accept = WebOperationContext.Current.IncomingRequest.Accept;
                info.UserAgent = WebOperationContext.Current.IncomingRequest.UserAgent;
                info.Headers = WebOperationContext.Current.IncomingRequest.Headers;

                OperationContext.Current.OutgoingMessageProperties.Add(
                    HttpRequestInformationProperty, info);
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState) { }
        }
    }
}
