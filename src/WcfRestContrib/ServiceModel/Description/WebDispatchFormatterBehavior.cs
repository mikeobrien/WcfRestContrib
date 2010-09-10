using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class WebDispatchFormatterBehavior : IOperationBehavior
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly WebDispatchFormatter.FormatterDirection _direction;

        // ────────────────────────── Constructors ──────────────────────────

        public WebDispatchFormatterBehavior(WebDispatchFormatter.FormatterDirection direction)
        {
            _direction = direction;
        }

        // ────────────────────────── IOperationBehavior Members ──────────────────────────

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            var behavior =
                operationDescription.DeclaringContract.Behaviors.Find
                    <WebDispatchFormatterConfigurationBehavior>();

            if (behavior == null)
                behavior = dispatchOperation.Parent.ChannelDispatcher.Host.Description.Behaviors.Find
                        <WebDispatchFormatterConfigurationBehavior>();

            if (behavior == null)
            {
                var configurationAttribute = 
                    operationDescription.DeclaringContract.GetAttribute<WebDispatchFormatterConfigurationAttribute>();

                if (configurationAttribute == null)
                    configurationAttribute = dispatchOperation.Parent.ChannelDispatcher.Host.Description.
                        GetAttribute<WebDispatchFormatterConfigurationAttribute>();

                string defaultMimeType = null;

                if (configurationAttribute != null)
                    defaultMimeType = configurationAttribute.DefaultMimeType;

                var mimeTypeAttributes = 
                    operationDescription.DeclaringContract.GetAttributes<WebDispatchFormatterMimeTypeAttribute>();

                if (mimeTypeAttributes == null || mimeTypeAttributes.Count == 0)
                    mimeTypeAttributes = dispatchOperation.Parent.ChannelDispatcher.Host.Description.
                        GetAttributes<WebDispatchFormatterMimeTypeAttribute>();

                var formatters = new Dictionary<string, Type>();

                if (mimeTypeAttributes != null && mimeTypeAttributes.Count > 0)
                {

                    foreach (var mimeTypeAttribute in mimeTypeAttributes)
                        foreach (var mimeType in mimeTypeAttribute.MimeTypes)
                        {
                            if (defaultMimeType == null) defaultMimeType = mimeType;
                            formatters.Add(mimeType, mimeTypeAttribute.Type);
                        }
                }

                if (formatters.Count > 0)
                    behavior = new WebDispatchFormatterConfigurationBehavior(
                        formatters, defaultMimeType);
            }

            if (behavior == null)
                throw new ConfigurationErrorsException(
                    "WebDispatchFormatterConfigurationBehavior or WebDispatchFormatterMimeTypeAttribute's not applied to contract or service. This behavior or attributes are required to configure web dispatch formatting.");

            dispatchOperation.Formatter = 
                new WebDispatchFormatter(
                    behavior.FormatterFactory,
                    operationDescription,
                    _direction != WebDispatchFormatter.FormatterDirection.Both ? dispatchOperation.Formatter : null,
                    _direction);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation) { }
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) { }
        public void Validate(OperationDescription operationDescription) { }
    }
}
