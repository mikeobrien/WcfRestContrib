using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class OperationAuthenticationInvoker : Attribute, IOperationInvoker 
    {
        private readonly IOperationInvoker _invoker;
        private readonly IWebAuthenticationHandler _handler;
        private readonly Type _validatorType;
        private readonly bool _requiresTransportLayerSecurity;
        private readonly string _source;

        public OperationAuthenticationInvoker(
            IOperationInvoker invoker,
            IWebAuthenticationHandler handler,
            Type validatorType,
            bool requiresTransportLayerSecurity,
            string source)
        {
            _invoker = invoker.ThrowIfNull();
            _handler = handler.ThrowIfNull();
            _validatorType = validatorType.ThrowIfNull();
            _requiresTransportLayerSecurity = requiresTransportLayerSecurity;
            _source = source;
        }

        public object[] AllocateInputs()
        { return _invoker.AllocateInputs(); }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            OperationContext.Current.ThrowIfNull().ReplacePrimaryIdentity(
                _handler.Authenticate(
                    WebOperationContext.Current.ThrowIfNull().IncomingRequest,
                    WebOperationContext.Current.ThrowIfNull().OutgoingResponse,
                    inputs,
                    _validatorType,
                    OperationContext.Current.ThrowIfNull().HasTransportLayerSecurity(),
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
