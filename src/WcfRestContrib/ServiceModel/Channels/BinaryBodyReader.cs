using System.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class BinaryBodyReader
    {
        public const string BinaryElementName = "Binary";
        private readonly byte[] _data;

        public BinaryBodyReader(XmlDictionaryReader reader)
        {
            reader.ReadStartElement(BinaryElementName);
            _data = reader.ReadContentAsBase64();
            if (reader.NodeType == XmlNodeType.Text) reader.Read();
            reader.ReadEndElement();
        }
        
        public byte[] Data 
        { get { return _data; } }
    }
}
