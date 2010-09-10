using System.Xml;

namespace WcfRestContrib.Xml
{
    public class PoxXmlWriter : XmlDictionaryWriter
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly XmlWriter _writer;
        private bool _inAttribute;

        // ────────────────────────── Constructors ──────────────────────────

        public PoxXmlWriter(XmlWriter writer)
        {
            _writer = writer;
        }


        // ────────────────────────── Overriden Members ──────────────────────────

        public override void WriteStartAttribute(string prefix, string localName, string ns) 
        {
            _inAttribute = true;
        }

        public override void WriteString(string text) 
        {
            if (!_inAttribute)
                _writer.WriteString(text);
        }

        public override void WriteEndAttribute() 
        {
            _inAttribute = false;
        }

        // ────────────────────────── Passthrough Members ──────────────────────────

        public override void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString namespaceUri) { base.WriteQualifiedName(localName, null); }
        public override void WriteQualifiedName(string localName, string ns) { base.WriteQualifiedName(localName, string.Empty); }
        public override void WriteStartElement(string prefix, string localName, string ns) { _writer.WriteStartElement(null, localName, null); }
        public override void WriteEndElement() { _writer.WriteEndElement(); }
        public override void WriteBase64(byte[] buffer, int index, int count) { _writer.WriteBase64(buffer, index, count); }
        public override void WriteCData(string text) { _writer.WriteCData(text); }
        public override void WriteChars(char[] buffer, int index, int count) { _writer.WriteChars(buffer, index, count); }
        public override void WriteRaw(string data) { _writer.WriteRaw(data); }
        public override void WriteRaw(char[] buffer, int index, int count) { _writer.WriteRaw(buffer, index, count); }
        public override string LookupPrefix(string ns) { return null; }
        public override void WriteCharEntity(char ch) { _writer.WriteCharEntity(ch); }
        public override void WriteComment(string text) { _writer.WriteComment(text); }
        public override void WriteDocType(string name, string pubid, string sysid, string subset) { _writer.WriteDocType(name, pubid, sysid, subset); }
        public override void WriteEntityRef(string name) { _writer.WriteEntityRef(name); }
        public override void WriteFullEndElement() { _writer.WriteFullEndElement(); }
        public override void WriteProcessingInstruction(string name, string text) { _writer.WriteProcessingInstruction(name, text); }
        public override void WriteStartDocument(bool standalone) { _writer.WriteStartDocument(standalone); }
        public override void WriteStartDocument() { _writer.WriteStartDocument(); }
        public override WriteState WriteState { get { return  _writer.WriteState; } }
        public override void WriteSurrogateCharEntity(char lowChar, char highChar) { _writer.WriteSurrogateCharEntity(lowChar, highChar); }
        public override void WriteWhitespace(string ws) { _writer.WriteWhitespace(ws); }
        public override void WriteEndDocument() { _writer.WriteEndDocument(); }
        public override void Close() { _writer.Close(); }
        public override void Flush() { _writer.Flush(); }
    }
}
