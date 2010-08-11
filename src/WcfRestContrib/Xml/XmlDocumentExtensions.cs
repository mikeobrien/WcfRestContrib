using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace WcfRestContrib.Xml
{
    public static class XmlDocumentExtensions
    {
        public static void SortAlphabetically<T>(this XmlDocument document) where T : XmlNode
        {
            document.FirstChild.SortAlphabetically<T>(true);
        }
    }
}
