using System;
using System.Runtime.Serialization;
using System.Xml;

namespace WcfRestContrib.Runtime.Serialization
{
    public class WrappedDataContractSerializer : XmlObjectSerializer
    {
        private readonly DataContractSerializer _serializer;
        private readonly bool _verifyObjectName;
        private readonly Func<XmlReader, XmlReader> _createReader;
        private readonly Func<XmlWriter, XmlWriter> _createWriter;

        public WrappedDataContractSerializer(Type type,
            Func<XmlReader, XmlReader> createReader,
            Func<XmlWriter, XmlWriter> createWriter,
            bool verifyObjectName) : 
            this(new DataContractSerializer(type),
            createReader,
            createWriter,
            verifyObjectName) { }

        public WrappedDataContractSerializer(
            DataContractSerializer serializer,
            Func<XmlReader, XmlReader> createReader,
            Func<XmlWriter, XmlWriter> createWriter,
            bool verifyObjectName)
        {
            _serializer = serializer;
            _verifyObjectName = verifyObjectName;
            _createReader = createReader;
            _createWriter = createWriter;
        }

        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            return _serializer.IsStartObject(_createReader(reader));
        }

        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            return _serializer.ReadObject(_createReader(reader), _verifyObjectName);
        }

        public override void WriteEndObject(XmlDictionaryWriter writer)
        {
            _serializer.WriteEndObject(_createWriter(writer));
        }

        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            _serializer.WriteObjectContent(_createWriter(writer), graph);
        }

        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {
            _serializer.WriteStartObject(_createWriter(writer), graph);
        }
    }
}
