using System;
using System.Collections.Generic;
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
    public class ServiceAuthenticationAttribute : Attribute, IServiceBehavior, IContractBehavior
    {
        // ────────────────────────── Private Fields ──────────────────────────

        ServiceAuthenticationBehavior _behavior = new ServiceAuthenticationBehavior();

        // ────────────────────────── IServiceBehavior Members ──────────────────────────

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters); }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.ApplyDispatchBehavior(serviceDescription, serviceHostBase); }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.Validate(serviceDescription, serviceHostBase); }

        // ────────────────────────── IContractBehavior Members ──────────────────────────

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(contractDescription, endpoint, bindingParameters); }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { _behavior.ApplyClientBehavior(contractDescription, endpoint, clientRuntime); }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        { _behavior.ApplyDispatchBehavior(contractDescription, endpoint, dispatchRuntime); }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        { _behavior.Validate(contractDescription, endpoint); }
    }
}
