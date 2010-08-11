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
    public class FormatterElement : ConfigurationElement
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string MIME_TYPES_ELEMENT = "mimeTypes";
        private const string TYPE_ELEMENT = "type";

        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _type;
        private static readonly ConfigurationProperty _mimeTypes;

        // ────────────────────────── Constructors ──────────────────────────

        static FormatterElement()
        {
            _type = new ConfigurationProperty(TYPE_ELEMENT, typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsRequired);
            _mimeTypes = new ConfigurationProperty(MIME_TYPES_ELEMENT, typeof(CommaDelimitedStringCollection), null, new CommaDelimitedStringCollectionConverter(), null, ConfigurationPropertyOptions.None);
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_type);
            _properties.Add(_mimeTypes);
        }

        public FormatterElement() { }

        // ────────────────────────── Public Members ──────────────────────────

        public CommaDelimitedStringCollection MimeTypes
        {
            get { return (CommaDelimitedStringCollection)this[_mimeTypes]; }
        }

        public string Type
        {
            get { return (string)this[_type]; }
        }

        // ────────────────────────── Private Members ──────────────────────────

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
    }
}
