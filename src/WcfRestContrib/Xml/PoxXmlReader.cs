using System.Xml;

namespace WcfRestContrib.Xml
{
    public class PoxXmlReader : XmlDictionaryReader
    {
        private readonly XmlReader _sourceReader;
        private XmlReader _reader;

        public PoxXmlReader() { }

        public PoxXmlReader(XmlReader reader)
        {
            _sourceReader = reader;
        }

        protected virtual XmlDocument GetDocument()
        {
            var document = new XmlDocument();
            if (_sourceReader != null) document.Load(_sourceReader);
            return document;
        }

        private XmlReader Reader
        {
            get
            {
                if (_reader == null)
                {
                    var document = GetDocument();
                    document.SortAlphabetically<XmlElement>();
                    _reader = new XmlNodeReader(document);
                }
                return _reader;
            }
        }

        public override bool IsStartElement(string localname, string ns) { return base.IsStartElement(localname); }
        public override string GetAttribute(string name, string namespaceUri) { return Reader.GetAttribute(name); }
        public override bool MoveToAttribute(string name, string ns) { return Reader.MoveToAttribute(name); }
        public override string ReadElementString(string localname, string ns) { return base.ReadElementString(localname); }
        public override void ReadStartElement(string localname, string ns) { base.ReadStartElement(localname); }
        public override bool ReadToDescendant(string localName, string namespaceUri) { return base.ReadToDescendant(localName); }
        public override bool ReadToFollowing(string localName, string namespaceUri) { return base.ReadToFollowing(localName); }
        public override bool ReadToNextSibling(string localName, string namespaceUri) { return base.ReadToNextSibling(localName); }
        public override string this[string name, string namespaceUri] { get { return base[name]; } }

        public override bool CanReadBinaryContent
        {
            get
            {
                return Reader.CanReadBinaryContent;
            }
        }

        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
        {
            return Reader.ReadContentAsBase64(buffer, index, count);
        }

        public override bool Read()
        {
            do
            {
                var more = Reader.Read();
                if (!more) return false;
            } while (Reader.IsEmptyElement);

            return true;
        }

        public override string NamespaceURI { get { return Reader.NamespaceURI; } }
        public override int AttributeCount { get { return Reader.AttributeCount; } }
        public override string BaseURI { get { return Reader.BaseURI; } }
        public override void Close() { Reader.Close(); }
        public override int Depth { get { return Reader.Depth; } }
        public override bool EOF { get { return Reader.EOF; } }
        public override string GetAttribute(int i) { return Reader.GetAttribute(i); }
        public override string GetAttribute(string name) { return Reader.GetAttribute(name); }
        public override bool HasValue { get { return Reader.HasValue; } }
        public override bool IsEmptyElement { get { return Reader.IsEmptyElement; } }
        public override string LocalName { get { return Reader.LocalName; } }
        public override string LookupNamespace(string prefix) { return Reader.Prefix; }
        public override bool MoveToAttribute(string name) { return Reader.MoveToAttribute(name); }
        public override bool MoveToElement() { return Reader.MoveToElement(); }
        public override bool MoveToFirstAttribute() { return Reader.MoveToFirstAttribute(); }
        public override bool MoveToNextAttribute() { return Reader.MoveToNextAttribute(); }
        public override XmlNameTable NameTable { get { return Reader.NameTable; } }
        public override XmlNodeType NodeType { get { return Reader.NodeType; } }
        public override string Prefix { get { return Reader.Prefix; } }
        public override bool ReadAttributeValue() { return Reader.ReadAttributeValue(); }
        public override ReadState ReadState { get { return Reader.ReadState; } }
        public override void ResolveEntity() { Reader.ResolveEntity(); }
        public override string Value { get { return Reader.Value; } }
    }
}
