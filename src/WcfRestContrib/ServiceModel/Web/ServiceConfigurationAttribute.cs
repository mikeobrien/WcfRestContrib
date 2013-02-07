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
            this(null, false, TransferMode.Buffered, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, TransferMode transferMode) :
            this(behaviorConfiguration, false, transferMode, InstanceContextMode.PerSession, ConcurrencyMode.Single, null) { }

        public ServiceConfigurationAttribute(TransferMode transferMode, params string[] bindingConfiguration) :
            this(null, false, transferMode, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler) :
            this(behaviorConfiguration, customErrorHandler, TransferMode.Buffered, InstanceContextMode.PerSession, ConcurrencyMode.Single, null) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, InstanceContextMode instanceContextMode, params string[] bindingConfiguration) :
            this(behaviorConfiguration, customErrorHandler, TransferMode.Buffered, instanceContextMode, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, params string[] bindingConfiguration) :
            this(behaviorConfiguration, customErrorHandler, TransferMode.Buffered, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(bool customErrorHandler, params string[] bindingConfiguration) :
            this(null, customErrorHandler, TransferMode.Buffered, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, TransferMode transferMode) :
            this(behaviorConfiguration, customErrorHandler, transferMode, InstanceContextMode.PerSession, ConcurrencyMode.Single, null) { }

        public ServiceConfigurationAttribute(bool customErrorHandler, TransferMode transferMode, params string[] bindingConfiguration) :
            this(null, customErrorHandler, transferMode, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, TransferMode transferMode, params string[] bindingConfiguration) :
            this(behaviorConfiguration, customErrorHandler, transferMode, InstanceContextMode.PerSession, ConcurrencyMode.Single, bindingConfiguration) { }

        public ServiceConfigurationAttribute(string behaviorConfiguration, bool customErrorHandler, TransferMode transferMode, 
            InstanceContextMode instanceContextMode, ConcurrencyMode concurencyMode, params string[] bindingConfiguration)
        {
            InstanceContextMode = instanceContextMode;
            ConcurrencyMode = concurencyMode;
            CustomErrorHandler = customErrorHandler;
            TransferMode = transferMode;
            BehaviorConfiguration = behaviorConfiguration;
            BindingConfiguration = bindingConfiguration;
        }

        public InstanceContextMode InstanceContextMode { get; set; }
        public ConcurrencyMode ConcurrencyMode { get; set; }
        public bool CustomErrorHandler { get; private set; }
        public TransferMode TransferMode { get; private set; }
        public string[] BindingConfiguration { get; private set; }
        public string BehaviorConfiguration { get; private set; }
    }
}
