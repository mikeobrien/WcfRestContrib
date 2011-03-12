using System;
using System.Configuration;

namespace WcfRestContrib.ServiceModel.Configuration.WebDispatchFormatter
{
    public class FormattersElement : ConfigurationElementCollection
    {
        private const string DefaultMimeTypeElement = "defaultMimeType";

        public FormattersElement()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            AddElementName = "formatter";
        }

        [ConfigurationProperty(DefaultMimeTypeElement)]
        public string DefaultMimeType
        {
            get { return (string)this[DefaultMimeTypeElement]; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FormatterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FormatterElement)element).Type;
        }
    }
}
