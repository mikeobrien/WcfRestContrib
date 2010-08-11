using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class WebException : Exception 
    {
        // ────────────────────────── Constructors ──────────────────────────

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

        // ────────────────────────── Public Members ──────────────────────────

        public HttpStatusCode Status { get; private set; }

        public virtual void UpdateHeaders(WebHeaderCollection headers) { }
    }
}
