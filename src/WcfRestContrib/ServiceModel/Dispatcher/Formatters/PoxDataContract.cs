using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using WcfRestContrib.Runtime.Serialization;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Channels;
using System.IO;
using WcfRestContrib.Xml;

namespace WcfRestContrib.ServiceModel.Dispatcher.Formatters
{
    public class PoxDataContract : IWebFormatter 
    {
        // ────────────────────────── IWebFormatter Members ──────────────────────────

        public object Deserialize(WebFormatterDeserializationContext context, Type type)
        {
            if (context.ContentFormat == WebFormatterDeserializationContext.DeserializationFormat.Xml)
            {
                return CreateSerializer(type).ReadObject(context.XmlReader);
            }
            else throw new InvalidDataException("Data must be in xml format.");
        }

        public WebFormatterSerializationContext Serialize(object data, Type type)
        {
            return WebFormatterSerializationContext.CreateXmlSerialized(CreateSerializer(type));
        }

        // ────────────────────────── Private Methods ──────────────────────────

        private WrappedDataContractSerializer CreateSerializer(Type type)
        {
            return new WrappedDataContractSerializer(type,
                        r => new PoxXmlReader(r),
                        w => new PoxXmlWriter(w),
                        false);
        }
    }
}
