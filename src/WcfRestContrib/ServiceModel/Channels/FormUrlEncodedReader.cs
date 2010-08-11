using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Web;

namespace WcfRestContrib.ServiceModel.Channels
{
    public class FormUrlEncodedReader : NameValueReader 
    {
        // ────────────────────────── Constructors ──────────────────────────

        public FormUrlEncodedReader(Stream stream):
            base(stream, "&", "=", ".") { }

        // ────────────────────────── Implemented Members ──────────────────────────

        public override string DecodeName(string name)
        {
            if (name != null && name.Contains('#'))
            {
                return name.Substring(0, name.IndexOf('#'));
            }
            else
                return name ?? string.Empty;
        }

        public override string DecodeValue(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
