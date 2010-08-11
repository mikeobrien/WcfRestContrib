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
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration.WebDispatchFormatter
{
    public class ConfigurationBehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── Private Fields ────────────────────────────────────

        private const string FORMATTERS_ELEMENT = "formatters";

        // ────────────────────────── BehaviorExtensionElement Overrides ────────────────

        public override Type BehaviorType
        {
            get { return typeof(WebDispatchFormatterConfigurationBehavior); }
        }

        protected override object CreateBehavior()
        {
            Dictionary<string, Type> formatters = new Dictionary<string, Type>();

            foreach (FormatterElement element in Formatters)
                foreach (string mimeType in element.MimeTypes)
                {
                    Type formatter = null;
                    try
                    {
                        formatter = element.Type.GetType<IWebFormatter>();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Invalid type specified in formatter behavior element. {0}", e));
                    }

                    formatters.Add(
                        mimeType,
                        formatter);
                }

            return new WebDispatchFormatterConfigurationBehavior(
                formatters, Formatters.DefaultMimeType);
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(FORMATTERS_ELEMENT, IsRequired = true)]
        public FormattersElement Formatters
        {
            get
            { return (FormattersElement)base[FORMATTERS_ELEMENT]; }
            set
            { base[FORMATTERS_ELEMENT] = value; }
        }
    }
}
