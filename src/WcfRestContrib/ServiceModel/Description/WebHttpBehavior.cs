using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebHttpBehavior : System.ServiceModel.Description.WebHttpBehavior
    {
        private readonly bool _customErrorHandler;

        public WebHttpBehavior(bool customErrorHandler)
        {
            _customErrorHandler = customErrorHandler;
        }

        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (!_customErrorHandler)
                base.AddServerErrorHandlers(endpoint, endpointDispatcher);
        } 
   
        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            IOperationBehavior originalBehavior = null; 
            IOperationBehavior surrogateBehavior = null;

            TryGetSurrogateBehavior(operationDescription,
                                    ref originalBehavior,
                                    ref surrogateBehavior);

            SwapBehaviors(operationDescription, originalBehavior, surrogateBehavior);

            var formatter = base.GetRequestDispatchFormatter(operationDescription, endpoint);

            SwapBehaviors(operationDescription, surrogateBehavior, originalBehavior);

            return formatter;
        }

        private static void SwapBehaviors(OperationDescription operationDescription, IOperationBehavior remove, IOperationBehavior add)
        {
            if (remove != null && add != null)
            {
                operationDescription.Behaviors.Remove(remove);
                operationDescription.Behaviors.Add(add);
            }
        }

        private static void TryGetSurrogateBehavior(OperationDescription operationDescription, ref IOperationBehavior original, ref IOperationBehavior surrogate)
        {
            if (!IsUntypedMessage(operationDescription.Messages[0]) && 
                operationDescription.Messages[0].Body.Parts.Count != 0)
            {
                var webGetAttribute = operationDescription.Behaviors.Find<WebGetAttribute>();
                if (webGetAttribute != null)
                {
                    original = webGetAttribute;
                    surrogate = new WebInvokeAttribute {
                         BodyStyle = webGetAttribute.BodyStyle,
                         Method = "NONE",
                         RequestFormat = webGetAttribute.RequestFormat,
                         ResponseFormat = webGetAttribute.ResponseFormat,
                         UriTemplate = webGetAttribute.UriTemplate };
                }
                else
                {
                    var webInvokeAttribute = operationDescription.Behaviors.Find<WebInvokeAttribute>();
                    if (webInvokeAttribute != null && webInvokeAttribute.Method == "GET")
                    {
                        original = webInvokeAttribute;
                        surrogate = new WebInvokeAttribute {
                            BodyStyle = webInvokeAttribute.BodyStyle,
                            Method = "NONE",
                            RequestFormat = webInvokeAttribute.RequestFormat,
                            ResponseFormat = webInvokeAttribute.ResponseFormat,
                            UriTemplate = webInvokeAttribute.UriTemplate };
                    }
                }
            }
        }

        private static bool IsUntypedMessage(MessageDescription message)
        {
            if (message == null)
            {
                return false;
            }
            return ((((message.Body.ReturnValue != null) && 
                (message.Body.Parts.Count == 0)) && 
                (message.Body.ReturnValue.Type == typeof(Message))) || 
                (((message.Body.ReturnValue == null) && (message.Body.Parts.Count == 1)) && 
                (message.Body.Parts[0].Type == typeof(Message))));
        }
    }
}
