using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel;

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
