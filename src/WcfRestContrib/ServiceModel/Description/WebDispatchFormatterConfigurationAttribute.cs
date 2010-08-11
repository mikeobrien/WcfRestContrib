using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfRestContrib.ServiceModel.Description
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false)]
    public class WebDispatchFormatterConfigurationAttribute : Attribute
    {
        public WebDispatchFormatterConfigurationAttribute(string defaultMimeType)
        {
            DefaultMimeType = defaultMimeType;
        }

        public string DefaultMimeType { get; private set; }
    }
}
