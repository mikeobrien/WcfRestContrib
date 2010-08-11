using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class BinaryBodyReader
    {
        // ────────────────────────── Public Constants ─────────────────────────

        public const string BINARY_ELEMENT_NAME = "Binary";

        // ────────────────────────── Private Fields ──────────────────────────

        private byte[] _data;

        // ────────────────────────── Constructors ─────────────────────────────

        public BinaryBodyReader(XmlDictionaryReader reader)
        {
            reader.ReadStartElement(BINARY_ELEMENT_NAME);
            _data = reader.ReadContentAsBase64();
            reader.ReadEndElement();
        }
        
        // ────────────────────────── Overriden Members ────────────────────────

        public byte[] Data 
        { get { return _data; } }
    }
}
