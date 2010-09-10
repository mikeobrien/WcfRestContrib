using System.Xml;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class WebFormatterDeserializationContext
    {
        // ────────────────────────── Public Constants ──────────────────────────

        public enum DeserializationFormat
        {
            Binary,
            Xml
        }

        // ────────────────────────── Constructors ──────────────────────────

        private WebFormatterDeserializationContext(byte[] binaryData)
        {
            BinaryData = binaryData;
            ContentFormat = DeserializationFormat.Binary;
        }

        private WebFormatterDeserializationContext(XmlDictionaryReader xmlReader)
        {
            ContentFormat = DeserializationFormat.Xml;
            XmlReader = xmlReader;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public DeserializationFormat ContentFormat { get; private set; }
        public byte[] BinaryData { get; private set; }
        public XmlDictionaryReader XmlReader { get; private set; }

        public static WebFormatterDeserializationContext CreateXml(XmlDictionaryReader xmlReader)
        {
            return new WebFormatterDeserializationContext(xmlReader);
        }

        public static WebFormatterDeserializationContext CreateBinary(byte[] binaryData)
        {
            return new WebFormatterDeserializationContext(binaryData);
        }
    }
}
