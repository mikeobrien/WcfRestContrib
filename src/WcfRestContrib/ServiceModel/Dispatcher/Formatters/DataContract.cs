using System;
using System.Runtime.Serialization;
using System.IO;

namespace WcfRestContrib.ServiceModel.Dispatcher.Formatters
{
    public class DataContract : IWebFormatter 
    {
        public object Deserialize(WebFormatterDeserializationContext context, Type type)
        {
            if (context.ContentFormat == WebFormatterDeserializationContext.DeserializationFormat.Xml)
            {
                var serializer = new DataContractSerializer(type);
                return serializer.ReadObject(context.XmlReader);
            }
            throw new InvalidDataException("Data must be in xml format.");
        }

        public WebFormatterSerializationContext Serialize(object data, Type type)
        {
            return WebFormatterSerializationContext.CreateXmlSerialized(new DataContractSerializer(type));
        }
    }
}
