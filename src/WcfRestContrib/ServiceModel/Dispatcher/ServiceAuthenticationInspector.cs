using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class ServiceAuthenticationInspector : Attribute, IDispatchMessageInspector 
    {
        private readonly IWebAuthenticationHandler _handler;
        private readonly Type _validatorType;
        private readonly bool _requiresTransportLayerSecurity;
        private readonly string _source;

        public ServiceAuthenticationInspector(
            IWebAuthenticationHandler handler,
            Type validatorType,
            bool requiresTransportLayerSecurity,
            string source)
        {
            _handler = handler;
            _validatorType = validatorType;
            _requiresTransportLayerSecurity = requiresTransportLayerSecurity;
            _source = source;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            OperationContext.Current.ReplacePrimaryIdentity(_handler.Authenticate(
                WebOperationContext.Current.IncomingRequest,
                WebOperationContext.Current.OutgoingResponse,
                new object[] {},
                _validatorType,
                OperationContext.Current.HasTransportLayerSecurity(),
                _requiresTransportLayerSecurity,
                _source));

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState) { }
    }
}
