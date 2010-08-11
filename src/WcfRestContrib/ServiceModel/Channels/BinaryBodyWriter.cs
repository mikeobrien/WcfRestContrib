using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class BinaryBodyWriter : BodyWriter
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private byte[] _data;

        // ────────────────────────── Constructors ──────────────────────────

        public BinaryBodyWriter(string data) : base(true)
        {
            _data = UTF8Encoding.UTF8.GetBytes(data);
        }

        public BinaryBodyWriter(byte[] data) : base(true)
        {
            _data = data;
        }

        // ────────────────────────── Overriden Members ──────────────────────────

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(BinaryBodyReader.BINARY_ELEMENT_NAME);
            writer.WriteBase64(_data, 0, _data.Length);
            writer.WriteEndElement();
        }
    }
}
