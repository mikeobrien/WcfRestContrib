using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using WcfRestContrib.Xml;
using System.Xml;

namespace WcfRestContrib.Runtime.Serialization
{
    public class WrappedDataContractSerializer : XmlObjectSerializer
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private System.Runtime.Serialization.DataContractSerializer _serializer;
        private bool _verifyObjectName;
        private Func<XmlReader, XmlReader> _createReader;
        private Func<XmlWriter, XmlWriter> _createWriter;

        // ────────────────────────── Constructors ──────────────────────────

        public WrappedDataContractSerializer(Type type,
            Func<XmlReader, XmlReader> createReader,
            Func<XmlWriter, XmlWriter> createWriter,
            bool verifyObjectName) : 
            this(new DataContractSerializer(type),
            createReader,
            createWriter,
            verifyObjectName) { }

        public WrappedDataContractSerializer(
            System.Runtime.Serialization.DataContractSerializer serializer,
            Func<XmlReader, XmlReader> createReader,
            Func<XmlWriter, XmlWriter> createWriter,
            bool verifyObjectName)
        {
            _serializer = serializer;
            _verifyObjectName = verifyObjectName;
            _createReader = createReader;
            _createWriter = createWriter;
        }

        // ────────────────────────── Implemented Members ──────────────────────────

        public override bool IsStartObject(System.Xml.XmlDictionaryReader reader)
        {
            return _serializer.IsStartObject(_createReader(reader));
        }

        public override object ReadObject(System.Xml.XmlDictionaryReader reader, bool verifyObjectName)
        {
            return _serializer.ReadObject(_createReader(reader), _verifyObjectName);
        }

        public override void WriteEndObject(System.Xml.XmlDictionaryWriter writer)
        {
            _serializer.WriteEndObject(_createWriter(writer));
        }

        public override void WriteObjectContent(System.Xml.XmlDictionaryWriter writer, object graph)
        {
            _serializer.WriteObjectContent(_createWriter(writer), graph);
        }

        public override void WriteStartObject(System.Xml.XmlDictionaryWriter writer, object graph)
        {
            _serializer.WriteStartObject(_createWriter(writer), graph);
        }
    }
}
