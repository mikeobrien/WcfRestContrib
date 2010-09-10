using System;
using System.Configuration;

namespace WcfRestContrib.ServiceModel.Configuration.WebDispatchFormatter
{
    public class FormattersElement : ConfigurationElementCollection
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string DefaultMimeTypeElement = "defaultMimeType";

        // ────────────────────────── Constructors ──────────────────────────

        public FormattersElement()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            AddElementName = "formatter";
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(DefaultMimeTypeElement)]
        public string DefaultMimeType
        {
            get { return (string)this[DefaultMimeTypeElement]; }
        }

        // ────────────────────────── Protected Members ──────────────────────────

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
