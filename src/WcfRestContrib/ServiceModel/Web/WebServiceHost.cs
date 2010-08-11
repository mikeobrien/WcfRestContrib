using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Web
{
    public class WebServiceHost : System.ServiceModel.Web.WebServiceHost
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private ServiceConfigurationAttribute _serviceConfigurationAttribute;

        // ────────────────────────── Constructors ──────────────────────────

        public WebServiceHost(
            Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public WebServiceHost(
            object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        // ────────────────────────── Overriden Members ──────────────────────────

        protected override void OnOpening()
        {
            base.OnOpening();

            // Load the service related items
            if (ServiceConfigurationAttribute != null)
            {
                this.ReplaceBehaviorOnAllEndpoints(
                    typeof (System.ServiceModel.Description.WebHttpBehavior),
                    new Description.WebHttpBehavior(
                        ServiceConfigurationAttribute.CustomErrorHandler));

                if (ServiceConfigurationAttribute.BindingConfiguration != null &&
                    ServiceConfigurationAttribute.BindingConfiguration.Length > 0 &&
                    !this.HasServiceElement())
                {
                    foreach (string bindingConfiguration in ServiceConfigurationAttribute.BindingConfiguration)
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
            foreach (KeyValuePair<string, ContractDescription> contract in this.ImplementedContracts)
            {
                var contractConfig = contract.Value.GetAttribute<ContractConfigurationAttribute>();
                if (contractConfig != null)
                    contract.Value.LoadContractBehaviors(contractConfig.BehaviorConfiguration);
            }
        }

        // ────────────────────────── Private Members ──────────────────────────

        private ServiceConfigurationAttribute ServiceConfigurationAttribute
        {
            get
            {
                if (_serviceConfigurationAttribute == null)
                    _serviceConfigurationAttribute =
                        this.GetServiceAttribute<ServiceConfigurationAttribute>();
                return _serviceConfigurationAttribute;
            }
        }
    }
}