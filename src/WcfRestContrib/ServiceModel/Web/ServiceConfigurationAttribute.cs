using System;
using System.ServiceModel;

namespace WcfRestContrib.ServiceModel.Web
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false, Inherited=true)]
    public class ServiceConfigurationAttribute : Attribute 
    {
        public ServiceConfigurationAttribute(bool customErrorHandler) :
            this(null, customErrorHandler, TransferMode.Buffered) { }

        public ServiceConfigurationAttribute(params string[] bindingConfiguration) :
            this(null, false, TransferMode.Buffered, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, TransferMode transferMode) :
            this(behaviorConfiguration, false, transferMode, null) { }

        public ServiceConfigurationAttribute(TransferMode transferMode, params string[] bindingConfiguration) :
            this(null, false, transferMode, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler) :
            this(behaviorConfiguration, customErrorHandler, TransferMode.Buffered, null) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, params string[] bindingConfiguration) :
            this(behaviorConfiguration, customErrorHandler, TransferMode.Buffered, bindingConfiguration) { }

        public ServiceConfigurationAttribute(bool customErrorHandler, params string[] bindingConfiguration) :
            this(null, customErrorHandler, TransferMode.Buffered, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, TransferMode transferMode) :
            this(behaviorConfiguration, customErrorHandler, transferMode, null) { }

        public ServiceConfigurationAttribute(bool customErrorHandler, TransferMode transferMode, params string[] bindingConfiguration) :
            this(null, customErrorHandler, transferMode, bindingConfiguration) { }
    
        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, TransferMode transferMode, params string[] bindingConfiguration)
        {
            CustomErrorHandler = customErrorHandler;
            TransferMode = transferMode;
            BehaviorConfiguration = behaviorConfiguration;
            BindingConfiguration = bindingConfiguration;
        }

        public bool CustomErrorHandler { get; private set; }
        public TransferMode TransferMode { get; private set; }
        public string[] BindingConfiguration { get; private set;}
        public string BehaviorConfiguration { get; private set;}
    }
}
