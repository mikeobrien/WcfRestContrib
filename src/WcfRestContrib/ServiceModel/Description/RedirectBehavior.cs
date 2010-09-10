using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class RedirectBehavior : IOperationBehavior 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly string _redirectUrlQuerystringParameter;

        // ────────────────────────── Constructors ──────────────────────────

        public RedirectBehavior(string redirectUrlQuerystringParameter)
        {
            _redirectUrlQuerystringParameter = redirectUrlQuerystringParameter;
        }

        // ────────────────────────── IOperationBehavior Members ──────────────────────────

        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
             dispatchOperation.ParameterInspectors.Add( 
                 new RedirectInspector(_redirectUrlQuerystringParameter));
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation) { }
        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters) { }
        public void Validate(OperationDescription operationDescription) { }
    }
}
