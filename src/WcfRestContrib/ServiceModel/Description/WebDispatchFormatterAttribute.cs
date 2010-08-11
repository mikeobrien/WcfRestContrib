using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebDispatchFormatterAttribute : Attribute, IOperationBehavior
    {
        // ────────────────────────── Private Fields ─────────────────────────────

        private WebDispatchFormatterBehavior _behavior;

        // ────────────────────────── Constructors ───────────────────────────────

        public WebDispatchFormatterAttribute() : this(WebDispatchFormatter.FormatterDirection.Both)
        {
        }

        public WebDispatchFormatterAttribute(WebDispatchFormatter.FormatterDirection direction)
        {
            _behavior = new WebDispatchFormatterBehavior(direction);
        }

        // ────────────────────────── IOperationBehavior Members ──────────────────

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