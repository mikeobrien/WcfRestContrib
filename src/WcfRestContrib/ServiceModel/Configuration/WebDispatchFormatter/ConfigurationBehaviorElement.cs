using System;
using System.Collections.Generic;
using System.ServiceModel.Configuration;
using System.Configuration;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration.WebDispatchFormatter
{
    public class ConfigurationBehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── Private Fields ────────────────────────────────────

        private const string FormattersElement = "formatters";

        // ────────────────────────── BehaviorExtensionElement Overrides ────────────────

        public override Type BehaviorType
        {
            get { return typeof(WebDispatchFormatterConfigurationBehavior); }
        }

        protected override object CreateBehavior()
        {
            var formatters = new Dictionary<string, Type>();

            foreach (FormatterElement element in Formatters)
                foreach (var mimeType in element.MimeTypes)
                {
                    Type formatter;
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

        [ConfigurationProperty(FormattersElement, IsRequired = true)]
        public FormattersElement Formatters
        {
            get
            { return (FormattersElement)base[FormattersElement]; }
            set
            { base[FormattersElement] = value; }
        }
    }
}
