using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Web
{
    public class WebServiceHost : System.ServiceModel.Web.WebServiceHost
    {
        private ServiceConfigurationAttribute _serviceConfigurationAttribute;

        public WebServiceHost(
            Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) { }

        public WebServiceHost(
            object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses) { }

        protected override void OnOpening()
        {
            base.OnOpening();

            // Load the service related items
            if (ServiceConfigurationAttribute != null)
            {
                var behavior = Description.Behaviors.Find<ServiceBehaviorAttribute>();
                if (behavior != null)
                {
                    behavior.ConcurrencyMode = ServiceConfigurationAttribute.ConcurrencyMode;
                    behavior.InstanceContextMode = ServiceConfigurationAttribute.InstanceContextMode;
                }

                this.ReplaceBehaviorOnAllEndpoints(
                    typeof (System.ServiceModel.Description.WebHttpBehavior),
                    new WebHttpBehavior(
                        ServiceConfigurationAttribute.CustomErrorHandler));

                if (ServiceConfigurationAttribute.BindingConfiguration != null &&
                    ServiceConfigurationAttribute.BindingConfiguration.Length > 0 &&
                    !this.HasServiceElement())
                {
                    foreach (var bindingConfiguration in ServiceConfigurationAttribute.BindingConfiguration)
                        this.LoadBinding(bindingConfiguration);
                }

                this.ApplyToAllEndpointBindingElements<HttpTransportBindingElement>(
                    e => e.TransferMode = ServiceConfigurationAttribute.TransferMode);

                this.ApplyToAllEndpointBindingElements<HttpsTransportBindingElement>(
                    e => e.TransferMode = ServiceConfigurationAttribute.TransferMode);

                if (!string.IsNullOrEmpty(ServiceConfigurationAttribute.BehaviorConfiguration) &&
                    !this.HasServiceElement())
                    this.LoadBehaviors(ServiceConfigurationAttribute.BehaviorConfiguration);
            }

            // Load contract behaviors
            foreach (var contract in ImplementedContracts)
            {
                var contractConfig = contract.Value.GetAttribute<ContractConfigurationAttribute>();
                if (contractConfig != null)
                    contract.Value.LoadContractBehaviors(contractConfig.BehaviorConfiguration);
            }
        }

        private ServiceConfigurationAttribute ServiceConfigurationAttribute
        {
            get {
                return _serviceConfigurationAttribute ??
                       (_serviceConfigurationAttribute = this.GetServiceAttribute<ServiceConfigurationAttribute>());
            }
        }
    }
}