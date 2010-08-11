using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Configuration;
using WcfRestContrib.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Configuration.WebDispatchFormatter
{
    public class FormattersElement : ConfigurationElementCollection
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string DEFAULT_MIME_TYPE_ELEMENT = "defaultMimeType";

        // ────────────────────────── Constructors ──────────────────────────

        public FormattersElement()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            base.AddElementName = "formatter";
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(DEFAULT_MIME_TYPE_ELEMENT)]
        public string DefaultMimeType
        {
            get { return (string)this[DEFAULT_MIME_TYPE_ELEMENT]; }
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
