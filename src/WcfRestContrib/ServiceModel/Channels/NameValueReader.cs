using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Web;
using WcfRestContrib.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public abstract class NameValueReader : PoxXmlReader
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly Stream _stream;
        private readonly string _pairSeperator;
        private readonly string _nameValueSeperator;
        private readonly string _nameEntitiesSeperator;

        // ────────────────────────── Constructors ──────────────────────────

        protected NameValueReader(Stream stream, string pairSeperator, string nameValueSeperator, string nameEntitiesSeperator) 
        { 
            _stream = stream;
            _pairSeperator = pairSeperator;
            _nameValueSeperator = nameValueSeperator;
            _nameEntitiesSeperator = nameEntitiesSeperator;
        }

        // ────────────────────────── Abstract Members ──────────────────────────

        public abstract string DecodeName(string name);
        public abstract string DecodeValue(string value);

        // ────────────────────────── Private Members ──────────────────────────

        protected override XmlDocument GetDocument()
        {
            return Decode(_stream, _pairSeperator, _nameValueSeperator, _nameEntitiesSeperator);
        }

        private XmlDocument Decode(Stream stream, string pairSeperator, string nameValueSeperator, string nameEntitiesSeperator)
        {
            var elementMap = new Dictionary<string,XmlElement>();
            var document = new XmlDocument();
            string formData;

            using (TextReader reader = new StreamReader(stream))
            { formData = reader.ReadToEnd(); }

            if (!formData.IsNullOrEmpty())
            {
                var pairs = formData.Split(new [] { pairSeperator }, StringSplitOptions.None);
                if (pairs.Length > 0)
                {
                    foreach (var pair in pairs)
                    {
                        var nameValuePair = pair.Split(new [] { nameValueSeperator }, StringSplitOptions.None);
                        if (nameValuePair.Length > 0 && !elementMap.ContainsKey(nameValuePair[0]))
                        {
                            DecodeNameValuePair(
                                document,
                                elementMap,
                                nameValuePair[0],
                                nameValuePair.Length > 1 ?
                                    HttpUtility.UrlDecode(nameValuePair[1]) :
                                    string.Empty,
                                    nameEntitiesSeperator);
                        }
                    }
                }
            }
            return document;
        }

        private void DecodeNameValuePair(XmlDocument document, Dictionary<string, XmlElement> elementMap, string name, string value, string nameEntitiesSeperator)
        {
            var nameEntities = name.Split(new [] { nameEntitiesSeperator }, StringSplitOptions.None);
            
            if (nameEntities.Length < 2) 
                throw new InvalidDataException(
                    string.Format("Form value name must contain at least two entities seperated by a '{0}'.", nameEntitiesSeperator));

            var currentElement = document.DocumentElement;

            for (var index = 0; index < nameEntities.Length; index++)
            {
                XmlElement nextElement = null;
                if (index == 0)
                {
                    if (currentElement == null)
                    {
                        currentElement = document.CreateElement(DecodeName(nameEntities[index]));
                        document.AppendChild(currentElement);
                    }
                }
                else
                {
                    var fullyQualifiedName = string.Join(nameEntitiesSeperator, nameEntities, 0, index + 1);
                    if (currentElement.HasChildNodes && elementMap.ContainsKey(fullyQualifiedName))
                        nextElement = elementMap[fullyQualifiedName];
                    if (nextElement == null)
                    {
                        nextElement = document.CreateElement(DecodeName(nameEntities[index]));
                        if (index == nameEntities.Length - 1)
                            nextElement.AppendChild(document.CreateTextNode(DecodeValue(value)));
                        currentElement.AppendChild(nextElement);
                        elementMap.Add(fullyQualifiedName, nextElement);
                    }
                    currentElement = nextElement;
                }
            }
        }
    }
}
