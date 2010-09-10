using System;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Description
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=true)]
    public class WebDispatchFormatterMimeTypeAttribute : Attribute
    {
        public WebDispatchFormatterMimeTypeAttribute(Type type, params string[] mimeTypes)
        {
            if (!type.CastableAs<IWebFormatter>())
                throw new Exception("type must implement IWebFormatter.");

            MimeTypes = mimeTypes;
            Type = type;
        }

        public string[] MimeTypes { get; private set; }
        public Type Type { get; private set; }
    }
}
