using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Channels;
using System.IO;

namespace WcfRestContrib.ServiceModel.Dispatcher.Formatters
{
    public class DataContract : IWebFormatter 
    {
        public object Deserialize(WebFormatterDeserializationContext context, Type type)
        {
            if (context.ContentFormat == WebFormatterDeserializationContext.DeserializationFormat.Xml)
            {
                DataContractSerializer serializer = new DataContractSerializer(type);
                return serializer.ReadObject(context.XmlReader);
            }
            else throw new InvalidDataException("Data must be in xml format.");
        }

        public WebFormatterSerializationContext Serialize(object data, Type type)
        {
            return WebFormatterSerializationContext.CreateXmlSerialized(new DataContractSerializer(type));
        }
    }
}
