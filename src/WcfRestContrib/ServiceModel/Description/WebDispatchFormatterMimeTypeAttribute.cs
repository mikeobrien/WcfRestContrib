using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                throw new Exception(string.Format("type must implement IWebFormatter.", type.Name));

            MimeTypes = mimeTypes;
            Type = type;
        }

        public string[] MimeTypes { get; private set; }
        public Type Type { get; private set; }
    }
}
