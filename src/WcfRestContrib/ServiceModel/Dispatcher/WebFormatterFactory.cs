using System;
using System.Collections.Generic;
using System.Linq;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class WebFormatterFactory
    {
        private readonly Dictionary<string, IWebFormatter> _formatters = new Dictionary<string, IWebFormatter>();
        private readonly string _defaultContentType;

        public WebFormatterFactory(Dictionary<string, Type> formatters, string defaultContentType)
        {
            foreach (var formatterDescription in formatters)
            {
                _formatters.Add(
                    formatterDescription.Key,
                    (IWebFormatter)Activator.CreateInstance(formatterDescription.Value));
            }
            _defaultContentType = defaultContentType;
        }

        public IWebFormatter CreateFormatter(string contentType)
        {
            string resolvedContentType;
            return CreateFormatter(new [] {contentType}, out resolvedContentType);
        }

        public IWebFormatter CreateFormatter(string[] contentTypes, out string resolvedContentType)
        {
            if (contentTypes != null)
            {
                var formatterFactory =
                    _formatters.FirstOrDefault(i => contentTypes.Contains(i.Key));
                if (formatterFactory.Key != null && formatterFactory.Value != null)
                {
                    var serializer = formatterFactory.Value;

                    resolvedContentType = formatterFactory.Key;
                    return serializer;
                }
            }

            string resolvedType;
            var defaultFormatter = CreateFormatter(new [] { _defaultContentType }, out resolvedType);
            resolvedContentType = resolvedType;
            return defaultFormatter;
        }
    }
}
