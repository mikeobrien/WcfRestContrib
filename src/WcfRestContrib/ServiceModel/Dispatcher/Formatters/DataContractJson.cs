using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Channels;
using System.IO;
using System.Runtime.Serialization;

namespace WcfRestContrib.ServiceModel.Dispatcher.Formatters
{
    public class DataContractJson : IWebFormatter
    {
        public object Deserialize(WebFormatterDeserializationContext context, Type type)
        {
            if (context.ContentFormat == WebFormatterDeserializationContext.DeserializationFormat.Xml)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                return serializer.ReadObject(context.XmlReader);
            }
            else throw new InvalidDataException("Data must be in xml format.");
        }

        public WebFormatterSerializationContext Serialize(object data, Type type)
        {
            return WebFormatterSerializationContext.CreateXmlSerialized(new DataContractJsonSerializer(type));
        }
    }
}
