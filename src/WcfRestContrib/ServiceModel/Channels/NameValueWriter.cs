using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public abstract class NameValueWriter : XmlDictionaryWriter
    {
        // ────────────────────────── Private Fields ──────────────────────────

        bool _inAttribute;
        bool _firstElement = true;
        readonly Stack<string> _fullyQualifiedName = new Stack<string>();
        readonly Dictionary<string, int> _elementIndex = new Dictionary<string, int>();
        readonly string _nameEntitiesSeperator;
        readonly string _pairSeperator;
        readonly string _nameValueSeperator;

        // ────────────────────────── Constructors ──────────────────────────

        protected NameValueWriter(string pairSeperator, string nameValueSeperator, string nameEntitiesSeperator)
        {
            _pairSeperator = pairSeperator;
            _nameValueSeperator = nameValueSeperator;
            _nameEntitiesSeperator = nameEntitiesSeperator;
        }

        // ────────────────────────── Abstract Methods ──────────────────────────

        protected abstract void Write(string nameValuePair);
        protected abstract string EncodeName(string name, int index);
        protected abstract string EncodeValue(string value);

        // ────────────────────────── Implemented Members ──────────────────────────

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            var fullName = string.Format("{0}{1}{2}", GetCurrentFullyQualifiedName(), _nameEntitiesSeperator, localName);
            string name;
            if (_elementIndex.ContainsKey(fullName))
            {
                _elementIndex[fullName]++;
                name = EncodeName(localName, _elementIndex[fullName]);
            }
            else
            {
                _elementIndex.Add(fullName, 1);
                name = EncodeName(localName, 1);
            }
            _fullyQualifiedName.Push(name);
        }

        public override void WriteString(string text)
        {
            if (!_inAttribute)
            {
                var nameValuePair = string.Empty;
                if (!_firstElement) nameValuePair += _pairSeperator;
                nameValuePair += string.Format("{0}{1}{2}", GetCurrentFullyQualifiedName(), _nameValueSeperator, EncodeValue(text));
                Write(nameValuePair);
                Flush();
                if (_firstElement) _firstElement = false;
            }
        }

        public override void WriteRaw(string data)
        {
            WriteString(data);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns) 
        { _inAttribute = true; }

        public override void WriteEndAttribute() 
        { _inAttribute = false; }

        public override void WriteEndElement() 
        { if (_fullyQualifiedName.Count > 0) _fullyQualifiedName.Pop(); }

        // ────────────────────────── Private Members ──────────────────────────

        private string GetCurrentFullyQualifiedName()
        { return string.Join(_nameEntitiesSeperator, _fullyQualifiedName.Reverse().ToArray()); }

        // ────────────────────────── Not Implemented ──────────────────────────

        public override void WriteBase64(byte[] buffer, int index, int count) { }
        public override void WriteCData(string text) { }
        public override void WriteChars(char[] buffer, int index, int count) { }
        public override void WriteRaw(char[] buffer, int index, int count) { }
        public override string LookupPrefix(string ns) { return null; }
        public override void WriteCharEntity(char ch) { }
        public override void WriteComment(string text) { }
        public override void WriteDocType(string name, string pubid, string sysid, string subset) { }
        public override void WriteEntityRef(string name) { }
        public override void WriteFullEndElement() { }
        public override void WriteProcessingInstruction(string name, string text) { }
        public override void WriteStartDocument(bool standalone) { }
        public override void WriteStartDocument() { }
        public override WriteState WriteState { get { return WriteState.Element; } }
        public override void WriteSurrogateCharEntity(char lowChar, char highChar) { }
        public override void WriteWhitespace(string ws) { }
        public override void WriteEndDocument() { }
    }
}
