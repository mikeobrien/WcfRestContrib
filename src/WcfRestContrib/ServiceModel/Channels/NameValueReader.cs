using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Web;
using WcfRestContrib.Xml;

namespace WcfRestContrib.ServiceModel.Channels
{
    public abstract class NameValueReader : PoxXmlReader
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private Stream _stream;
        private string _pairSeperator;
        private string _nameValueSeperator;
        private string _nameEntitiesSeperator;

        // ────────────────────────── Constructors ──────────────────────────

        public NameValueReader(Stream stream, string pairSeperator, string nameValueSeperator, string nameEntitiesSeperator) 
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
            Dictionary<string, XmlElement> elementMap = new Dictionary<string,XmlElement>();
            XmlDocument document = new XmlDocument();
            string formData;

            using (TextReader reader = new StreamReader(stream))
            { formData = reader.ReadToEnd(); }

            if (formData != null && formData.Length > 0)
            {
                string[] pairs = formData.Split(new string[] { pairSeperator }, StringSplitOptions.None);
                if (pairs.Length > 0)
                {
                    foreach (string pair in pairs)
                    {
                        string[] nameValuePair = pair.Split(new string[] { nameValueSeperator }, StringSplitOptions.None);
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
            string[] nameEntities = name.Split(new string[] { nameEntitiesSeperator }, StringSplitOptions.None);
            
            if (nameEntities.Length < 2) 
                throw new InvalidDataException(
                    string.Format("Form value name must contain at least two entities seperated by a '{0}'.", nameEntitiesSeperator));

            XmlElement currentElement = document.DocumentElement;

            for (int index = 0; index < nameEntities.Length; index++)
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
                    string fullyQualifiedName = string.Join(nameEntitiesSeperator, nameEntities, 0, index + 1);
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
