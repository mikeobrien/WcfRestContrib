using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Net;
using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.Security.Principal;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class ServiceAuthenticationInspector : Attribute, IDispatchMessageInspector 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private IWebAuthenticationHandler _handler;
        private UserNamePasswordValidator _validator;
        private bool _requiresTransportLayerSecurity;
        private string _source;

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
