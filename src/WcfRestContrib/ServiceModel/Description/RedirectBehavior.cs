using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class RedirectBehavior : IOperationBehavior 
    {
        private readonly string _redirectUrlQuerystringParameter;

        public RedirectBehavior(string redirectUrlQuerystringParameter)
        {
            _redirectUrlQuerystringParameter = redirectUrlQuerystringParameter;
        }

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
