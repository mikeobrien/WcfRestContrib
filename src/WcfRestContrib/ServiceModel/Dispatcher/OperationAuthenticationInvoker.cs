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
    public class OperationAuthenticationInvoker : Attribute, IOperationInvoker 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private IOperationInvoker _invoker;
        private IWebAuthenticationHandler _handler;
        private UserNamePasswordValidator _validator;
        private bool _requiresTransportLayerSecurity;
        private string _source;

        // ────────────────────────── Constructors ──────────────────────────

        public OperationAuthenticationInvoker(
            IOperationInvoker invoker,
            IWebAuthenticationHandler handler,
            UserNamePasswordValidator validator,
            bool requiresTransportLayerSecurity,
            string source)
        {
            _invoker = invoker;
            _handler = handler;
            _validator = validator;
            _requiresTransportLayerSecurity = requiresTransportLayerSecurity;
            _source = source;
        }

        // ────────────────────────── IOperationInvoker Members ──────────────────────────

        public object[] AllocateInputs()
        { return _invoker.AllocateInputs(); }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            OperationContext.Current.ReplacePrimaryIdentity(_handler.Authenticate(
                WebOperationContext.Current.IncomingRequest,
                WebOperationContext.Current.OutgoingResponse,
                inputs,
                _validator,
                OperationContext.Current.HasTransportLayerSecurity(),
                _requiresTransportLayerSecurity,
                _source));

            return _invoker.Invoke(instance, inputs, out outputs);
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, 
            AsyncCallback callback, object state)
        { throw new NotSupportedException(); }

        public object InvokeEnd(object instance, out object[] outputs, 
            IAsyncResult result)
        { throw new NotSupportedException(); }

        public bool IsSynchronous
        { get { return true; } }
    }
}
