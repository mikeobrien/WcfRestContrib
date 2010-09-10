using System.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class BinaryBodyReader
    {
        // ────────────────────────── Public Constants ─────────────────────────

        public const string BinaryElementName = "Binary";

        // ────────────────────────── Private Fields ──────────────────────────

        private readonly byte[] _data;

        // ────────────────────────── Constructors ─────────────────────────────

        public BinaryBodyReader(XmlDictionaryReader reader)
        {
            reader.ReadStartElement(BinaryElementName);
            _data = reader.ReadContentAsBase64();
            reader.ReadEndElement();
        }
        
        // ────────────────────────── Overriden Members ────────────────────────

        public byte[] Data 
        { get { return _data; } }
    }
}
