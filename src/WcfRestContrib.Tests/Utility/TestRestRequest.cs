using System.Collections.Specialized;
using System.Net;

namespace WcfRestContrib.Tests.Utility
{
    /// <summary>
    /// Settings we might want to change in a test request
    /// </summary>
    public class TestRestRequest
    {
        public TestRestRequest(string verb, string uri)
        {
            Verb = verb;
            Uri = uri;
            ContentType = "text/xml";
        }

        public string Accept { get; set; }
        public string Verb { get; set; }
        public string Uri { get; set; }
        public ICredentials Credentials { get; set; }
        public NameValueCollection Headers { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
    }
}
