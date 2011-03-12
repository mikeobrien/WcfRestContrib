using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebDispatchFormatterAttribute : Attribute, IOperationBehavior
    {
        private readonly WebDispatchFormatterBehavior _behavior;

        public WebDispatchFormatterAttribute() : this(WebDispatchFormatter.FormatterDirection.Both)
        {
        }

        public WebDispatchFormatterAttribute(WebDispatchFormatter.FormatterDirection direction)
        {
            _behavior = new WebDispatchFormatterBehavior(direction);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            _behavior.ApplyDispatchBehavior(operationDescription, dispatchOperation);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            _behavior.ApplyClientBehavior(operationDescription, clientOperation);
        }

        public void AddBindingParameters(OperationDescription operationDescription,
                                         BindingParameterCollection bindingParameters)
        {
            _behavior.AddBindingParameters(operationDescription, bindingParameters);
        }

        public void Validate(OperationDescription operationDescription)
        {
            _behavior.Validate(operationDescription);
        }
    }
}