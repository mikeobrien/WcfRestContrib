using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class WebFormatterFactory
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private Dictionary<string, IWebFormatter> _formatters = 
                    new Dictionary<string, IWebFormatter>();
        private string _defaultContentType;

        // ────────────────────────── Constructors ──────────────────────────

        public WebFormatterFactory(Dictionary<string, Type> formatters, string defaultContentType)
        {
            foreach (KeyValuePair<string, Type> formatterDescription in formatters)
            {
                _formatters.Add(
                    formatterDescription.Key,
                    (IWebFormatter)Activator.CreateInstance(formatterDescription.Value));
            }
            _defaultContentType = defaultContentType;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public IWebFormatter CreateFormatter(string contentType)
        {
            string resolvedContentType;
            return CreateFormatter(new string[] {contentType}, out resolvedContentType);
        }

        public IWebFormatter CreateFormatter(string[] contentTypes, out string resolvedContentType)
        {
            if (contentTypes != null)
            {
                KeyValuePair<string, IWebFormatter> formatterFactory =
                    _formatters.FirstOrDefault(i => contentTypes.Contains(i.Key));
                if (formatterFactory.Key != null && formatterFactory.Value != null)
                {
                    IWebFormatter serializer = formatterFactory.Value;

                    resolvedContentType = formatterFactory.Key;
                    return serializer;
                }
            }

            string resolvedType;
            IWebFormatter defaultFormatter = CreateFormatter(new string[] { _defaultContentType }, out resolvedType);
            resolvedContentType = resolvedType;
            return defaultFormatter;
        }
    }
}
