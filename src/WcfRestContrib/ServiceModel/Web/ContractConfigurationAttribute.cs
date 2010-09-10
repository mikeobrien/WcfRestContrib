using System;

namespace WcfRestContrib.ServiceModel.Web
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false, Inherited=true)]
    public class ContractConfigurationAttribute : Attribute 
    {
        public ContractConfigurationAttribute(string behaviorConfiguration)
        {
            BehaviorConfiguration = behaviorConfiguration;
        }

        public string BehaviorConfiguration { get; private set;}
    }
}
