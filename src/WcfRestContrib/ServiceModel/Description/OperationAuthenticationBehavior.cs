using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Net;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class OperationAuthenticationBehavior : IOperationBehavior 
    {
        // ────────────────────────── IOperationBehavior Members ──────────────────────────

        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            WebAuthenticationConfigurationBehavior behavior =
                operationDescription.DeclaringContract.FindBehavior
                    <WebAuthenticationConfigurationBehavior,
                    WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                behavior = dispatchOperation.Parent.ChannelDispatcher.Host.Description.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                        WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ConfigurationErrorsException(
                    "OperationAuthenticationConfigurationBehavior not applied to contract or service. This behavior is required to configure operation authentication.");

            dispatchOperation.Invoker = new OperationAuthenticationInvoker(
                dispatchOperation.Invoker,
                behavior.AuthenticationHandler,
                behavior.UsernamePasswordValidator,
                behavior.RequireSecureTransport,
                behavior.Source);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation) { }
        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters) { }
        public void Validate(OperationDescription operationDescription) { }
    }
}
