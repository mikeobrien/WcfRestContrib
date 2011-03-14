using System.Configuration;
using System.IdentityModel.Selectors;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Description
{
    public class OperationAuthenticationBehavior : IOperationBehavior 
    {
        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            var behavior =
                operationDescription.DeclaringContract.FindBehavior
                    <WebAuthenticationConfigurationBehavior,
                    WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior) ??
                dispatchOperation.Parent.ChannelDispatcher.Host.Description.FindBehavior
                    <WebAuthenticationConfigurationBehavior,
                    WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ConfigurationErrorsException(
                    "OperationAuthenticationConfigurationBehavior not applied to contract or service. This behavior is required to configure operation authentication.");

            dispatchOperation.Invoker = new OperationAuthenticationInvoker(
                dispatchOperation.Invoker,
                ServiceLocator.Current.Create<IWebAuthenticationHandler>(behavior.AuthenticationHandler),
                ServiceLocator.Current.Create<UserNamePasswordValidator>(behavior.UsernamePasswordValidator),
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
