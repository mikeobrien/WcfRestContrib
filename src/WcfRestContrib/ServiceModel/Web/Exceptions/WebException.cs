using System;
using System.Net;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class WebException : Exception 
    {
        public WebException(HttpStatusCode status, string friendyMessage, params object[] args) : 
            base(string.Format(friendyMessage, args))
        {
            Status = status;
        }
    
        public WebException(Exception innerException, HttpStatusCode status, string friendyMessage, params object[] args) : 
            base(string.Format(friendyMessage, args), innerException)
        {
            Status = status;
        }             

        public HttpStatusCode Status { get; private set; }

        public virtual void UpdateHeaders(WebHeaderCollection headers) { }
    }
}
