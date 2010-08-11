using System.Net;

namespace WcfRestContrib.Tests.Utility
{
    public static class HttpWebRequestExtensions
    {
        public static string AsDebugString(this HttpWebRequest request)
        {
            return string.Format("{0} {1}", request.Method, request.Address);
        }
    }
}
