using System.Linq;
using System.IO;
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
            return name ?? string.Empty;
        }

        public override string DecodeValue(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
