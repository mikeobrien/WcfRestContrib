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
    public class FormUrlEncodedWriter : NameValueWriter
    {
        // ────────────────────────── Private Fields ──────────────────────────

        TextWriter _writer;

        // ────────────────────────── Constructors ──────────────────────────

        public FormUrlEncodedWriter(Stream stream) : base("&", "=", ".")
        { _writer = new StreamWriter(stream); }

        // ────────────────────────── Implemented Members ──────────────────────────

        protected override string EncodeName(string name, int index)
        {
            return index == 1 ? name : string.Format("{0}#{1}", name, index);
        }

        protected override string EncodeValue(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        protected override void Write(string nameValuePair)
        {
            _writer.Write(nameValuePair);
        }

        public override void Close() 
        { _writer.Close(); }

        public override void Flush() 
        { _writer.Flush(); }
    }
}
