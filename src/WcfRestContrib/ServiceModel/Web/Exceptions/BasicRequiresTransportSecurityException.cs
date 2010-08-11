using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfRestContrib.Net.Http;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class BasicRequiresTransportSecurityException : WebException 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private string _realm;

        // ────────────────────────── Constructors ──────────────────────────

        public BasicRequiresTransportSecurityException(string realm)
            : base(
                System.Net.HttpStatusCode.Forbidden,
                "This resource must be accessed over SSL/TLS.")
        {
            _realm = realm;
        }
    }
}
