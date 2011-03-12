using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class BinaryBodyWriter : BodyWriter
    {
        private readonly byte[] _data;

        public BinaryBodyWriter(string data) : base(true)
        {
            _data = Encoding.UTF8.GetBytes(data);
        }

        public BinaryBodyWriter(byte[] data) : base(true)
        {
            _data = data;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(BinaryBodyReader.BinaryElementName);
            writer.WriteBase64(_data, 0, _data.Length);
            writer.WriteEndElement();
        }
    }
}
