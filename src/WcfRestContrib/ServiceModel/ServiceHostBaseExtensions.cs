using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Configuration;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel
{
    public static class ServiceHostBaseExtensions
    {
        public static bool HasServiceElement(this ServiceHostBase serviceHost)
        {
            return (GetServiceElement(serviceHost) != null);
        }

        public static ServiceElement GetServiceElement(this ServiceHostBase serviceHost)
        {
            return ConfigurationManager.GetServiceElement(serviceHost.Description.ConfigurationName);
        }

        public static ServiceBehaviorElement GetServiceBehaviorElement(this ServiceHostBase serviceHost)
        {
            ServiceElement service = GetServiceElement(serviceHost);
            if (service != null)
                return ConfigurationManager.GetServiceBehaviorElement(service.BehaviorConfiguration);
            else
                return null;
        }

        public static void LoadBehaviors(
            this ServiceHostBase serviceHost,
            string behaviorConfiguration)
        {
            if (string.IsNullOrEmpty(behaviorConfiguration)) 
                throw new ArgumentException("Behavior configuration not specified.");

            ServiceBehaviorElement serviceBehaviors = 
                ConfigurationManager.GetServiceBehaviorElement(behaviorConfiguration);
            if (serviceBehaviors != null)
            {
                foreach (BehaviorExtensionElement behaviorExtension in serviceBehaviors)
                {
                    object extension = behaviorExtension.CreateBehavior();
                    if (extension != null)
                    {
                        Type extensionType = extension.GetType();
                        if (typeof(IServiceBehavior).IsAssignableFrom(extensionType))
                        {
                            if (serviceHost.Description.Behaviors.Contains(extensionType))
                            {
                                serviceHost.Description.Behaviors.Remove(extensionType);
                            }
                            serviceHost.Description.Behaviors.Add((IServiceBehavior)extension);
                        }
                    }
                }
            }
        }

        public static void LoadBinding(
            this ServiceHostBase serviceHost,
            string bindingConfiguration)
        {
            if (string.IsNullOrEmpty(bindingConfiguration))
                throw new ArgumentException("Binding configuration not specified.");

            BindingsSection bindingsSection = ConfigurationManager.GetBindingsSection();

            foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
            {
                // Get the binding collection of the same type as the endpoint binding
                BindingCollectionElement bindingCollection = bindingsSection.BindingCollections.FirstOrDefault(
                    b => b.BindingType == endpoint.Binding.GetType());

                // If there isnt one declared of that type move on
                if (bindingCollection == null) continue;

                // Get the binding configuration that matches the name passed in
                var configurationElement = bindingCollection.ConfiguredBindings.FirstOrDefault(
                    e => e.Name.Equals(
                            bindingConfiguration, 
                            StringComparison.Ordinal));

                // If there isnt one with the names specified move on
                if (configurationElement == null) continue;
                
                if (configurationElement is CustomBindingElement)
                {
                    CustomBinding binding = (CustomBinding)endpoint.Binding;
                    CustomBindingElement bindingElement = (CustomBindingElement)configurationElement;

                    // We only want to apply bindings if they have the
                    // same or no transport binding element

                    // Grab the configuration transport binding element
                    BindingElementExtensionElement configurationBindingElement = 
                        bindingElement.FirstOrDefault(e => e.BindingElementType.IsSubclassOf(typeof(TransportBindingElement)));

                    // If the binding configuration does indeed contain a 
                    // transport binding element then compare with the endpoint
                    // transport binding element otherwise dont worry about it
                    if (configurationBindingElement != null)
                    {
                        // Grab the endpoint transport binding element
                        TransportBindingElement transportBindingElement =
                            (TransportBindingElement)binding.Elements.
                            FirstOrDefault(e => e is TransportBindingElement);

                        // If these do not match move on otherwise we can 
                        // apply the binding configuration
                        if (transportBindingElement != null &&
                            configurationBindingElement.BindingElementType != transportBindingElement.GetType())
                                continue;
                    }

                    // The custom binding element will only add new binding 
                    // elements it will not remove existing ones first.
                    // This removes existing ones so we dont end up with duplicates.
                    bindingElement.RemoveDuplicateBindingExtensions(binding);
                }

                configurationElement.ApplyConfiguration(endpoint.Binding);
            }
        }

        public static void AddBehaviorOnAllEndpoints(
            this ServiceHostBase serviceHost,
            IEndpointBehavior behavior)
        {
            ReplaceBehaviorOnAllEndpoints(
                serviceHost,
                null,
                behavior);
        }

        public static void ReplaceBehaviorOnAllEndpoints(
            this ServiceHostBase serviceHost, 
            Type replaceType,
            IEndpointBehavior behavior)
        {
            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                if (replaceType != null)
                {
                    IEndpointBehavior exisitingBehavior = endpoint.Behaviors.FirstOrDefault(
                        b => b.GetType() == replaceType);
                    if (exisitingBehavior != null)
                        endpoint.Behaviors.Remove(exisitingBehavior);
                }
                endpoint.Behaviors.Add(behavior);
            }
        }
        
        public static void ApplyToAllEndpointBindingElements<TElement>(
            this ServiceHostBase serviceHost, 
            Action<TElement> action) where TElement:BindingElement
        {
            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                if (!(endpoint.Binding is CustomBinding))
                    endpoint.Binding = new CustomBinding(endpoint.Binding);
                TElement element = ((CustomBinding)endpoint.Binding).
                                        Elements.Find<TElement>();
                if (element != null) action(element);
            }
        }
        
        public static T GetServiceAttribute<T>(this ServiceHostBase serviceHost) where T:Attribute
        {
            return serviceHost.Description.GetAttribute<T>();
        }
    }
}
