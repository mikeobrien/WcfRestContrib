using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class ServiceAuthenticationInspector : Attribute, IDispatchMessageInspector 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly IWebAuthenticationHandler _handler;
        private readonly UserNamePasswordValidator _validator;
        private readonly bool _requiresTransportLayerSecurity;
        private readonly string _source;

        // ────────────────────────── Constructors ──────────────────────────

        public ServiceAuthenticationInspector(
            IWebAuthenticationHandler handler,
            UserNamePasswordValidator validator,
            bool requiresTransportLayerSecurity,
            string source)
        {
            _handler = handler;
            _validator = validator;
            _requiresTransportLayerSecurity = requiresTransportLayerSecurity;
            _source = source;
        }

        // ────────────────────────── IDispatchMessageInspector Members ──────────────────────────

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            OperationContext.Current.ReplacePrimaryIdentity(_handler.Authenticate(
                WebOperationContext.Current.IncomingRequest,
                WebOperationContext.Current.OutgoingResponse,
                new object[] {},
                _validator,
                OperationContext.Current.HasTransportLayerSecurity(),
                _requiresTransportLayerSecurity,
                _source));

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState) { }
    }
}
