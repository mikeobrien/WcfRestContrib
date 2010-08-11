using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using WcfRestContrib.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;
using System.Net.Mime;
using System.Runtime.Serialization;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.Net.Http;
using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace WcfRestContrib.ServiceModel.Web
{
    public class WebErrorHandler : IErrorHandler 
    {
        // ────────────────────────── IErrorHandler Members ──────────────────────────

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            WebException webException;

            if (error is WebException)
                webException = (WebException)error;
            else
            {
                WebErrorHandlerConfigurationBehavior behavior =
                        GetWebErrorHandlerConfiguration();

                string unhandledErrorMessage;
                
                if (behavior.ReturnRawException) unhandledErrorMessage = error.ToString();
                else unhandledErrorMessage = (behavior != null && behavior.UnhandledErrorMessage != null) ? behavior.UnhandledErrorMessage : "An error has occured processing your request.";

                webException = new WebException(error,
                    System.Net.HttpStatusCode.InternalServerError, 
                    unhandledErrorMessage);
            }

            webException.UpdateHeaders(WebOperationContext.Current.OutgoingResponse.Headers);
            WebOperationContext.Current.OutgoingResponse.StatusCode = webException.Status;
            WebOperationContext.Current.OutgoingResponse.StatusDescription = webException.Status.ToString();

            WebDispatchFormatter webDispatchFormatter = null;

            if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(WebDispatchFormatter.WebDispatcherFormatterProperty))
                webDispatchFormatter = OperationContext.Current.OutgoingMessageProperties[WebDispatchFormatter.WebDispatcherFormatterProperty] as WebDispatchFormatter;

            if (webDispatchFormatter != null)
            {
                WebErrorHandlerConfigurationBehavior behavior = GetWebErrorHandlerConfiguration();
                IWebExceptionDataContract exceptionContract;
                
                if (behavior != null && behavior.HasExceptionDataContract)
                    exceptionContract = behavior.CreateExceptionDataContract();
                else
                    exceptionContract = new WebExceptionContract();

                exceptionContract.Init(webException);

                fault = webDispatchFormatter.Serialize(
                    exceptionContract,
                    typeof(WebExceptionContract),
                    WebOperationContext.Current.IncomingRequest.GetAcceptTypes());
            }

            else
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = MediaTypeNames.Text.Html;

                fault = Message.CreateMessage(
                    MessageVersion.None,
                    null,
                    new BinaryBodyWriter(GenerateResponseText(webException.Message)));

                fault.SetWebContentFormatProperty(WebContentFormat.Raw);
            }

            fault.UpdateHttpProperty();
        }

        public bool HandleError(Exception error)
        {
            InternalHandleError(error);
            return true;
        }

        // ────────────────────────── Private Members ──────────────────────────

        private WebErrorHandlerConfigurationBehavior GetWebErrorHandlerConfiguration()
        {
            WebErrorHandlerConfigurationBehavior behavior = 
                    OperationContext.Current.Host.Description.Behaviors.
                    Find<WebErrorHandlerConfigurationBehavior>();

            if (behavior == null)
            {
                WebErrorHandlerConfigurationAttribute attribute =
                    OperationContext.Current.Host.Description.Behaviors.
                    Find<WebErrorHandlerConfigurationAttribute>();
                if (attribute != null) behavior = attribute.BaseBehavior;
            }
            return behavior;
        }

        private void InternalHandleError(Exception error)
        {
            WebErrorHandlerConfigurationBehavior behavior =
                    GetWebErrorHandlerConfiguration();

            if (behavior != null && behavior.LogHandler != null)
            {
                RequestInformation info;

                if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(
                    ErrorHandlerBehavior.HttpRequestInformationProperty))
                    info = (RequestInformation)OperationContext.Current.OutgoingMessageProperties[
                        ErrorHandlerBehavior.HttpRequestInformationProperty];
                else
                    info = new RequestInformation();

                behavior.LogHandler.Write(error, info);
            }
        }

        private string GenerateResponseText(string message)
        {
            return string.Format(
                "<html><body style=\"font-family:Arial;font-size:11pt;\">{0}</body></html>",
                message);
        }

        // ────────────────────────── Private Types ──────────────────────────

        [DataContract(Name="Error", Namespace="")]
        private class WebExceptionContract : IWebExceptionDataContract
        {
            [DataMember(Name = "Status")]
            public int Status { get; set; }

            [DataMember(Name = "Message")]
            public string Message { get; set; }

            // ────────────────────────── IWebExceptionContract Implementation ──────────────────────────

            public void Init(WebException exception)
            {
                Message = exception.Message;
                Status = (int)exception.Status;
            }
        }
    }
}
