using System;
using System.Runtime.Serialization;
using WcfRestContrib.ServiceModel.Channels;
using System.IO;

namespace WcfRestContrib.ServiceModel.Dispatcher.Formatters
{
    public class FormUrlEncoded : IWebFormatter
    {
        public object Deserialize(WebFormatterDeserializationContext context, Type type)
        {
            if (context.ContentFormat == WebFormatterDeserializationContext.DeserializationFormat.Binary)
            {
                FormUrlEncodedReader formReader = new FormUrlEncodedReader(new MemoryStream(context.BinaryData));
                DataContractSerializer serializer = new DataContractSerializer(type);
                return serializer.ReadObject(formReader, false);
            }
            throw new InvalidDataException("Data must be in binary format.");
        }

        public WebFormatterSerializationContext Serialize(object data, Type type)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(type);
            serializer.WriteObject(new FormUrlEncodedWriter(stream), data);
            stream.Position = 0;
            return WebFormatterSerializationContext.CreateBinary(
                new BinaryReader(stream).ReadBytes((int)stream.Length));
        }
    }
}
