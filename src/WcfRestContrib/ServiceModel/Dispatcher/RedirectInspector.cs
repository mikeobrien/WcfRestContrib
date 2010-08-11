using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Net;
using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.Security.Principal;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class RedirectInspector : Attribute, IParameterInspector 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private string _redirectUrlQuerystringParameter;

        // ────────────────────────── Constructors ──────────────────────────

        public RedirectInspector(
            string redirectUrlQuerystringParameter)
        {
            _redirectUrlQuerystringParameter = redirectUrlQuerystringParameter;
        }

        // ────────────────────────── IParameterInspector Members ──────────────────────────

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            // Check to see if there is a redirect parameter set in the attribute 
            // and that its specified in the uri
            string redirectUrl = GetRedirectUrl(_redirectUrlQuerystringParameter);
            if (redirectUrl != null && redirectUrl.Trim().Length > 0 &&
                WebOperationContext.Current.OutgoingResponse.StatusCode == HttpStatusCode.OK) 
                WebOperationContext.Current.OutgoingResponse.Redirect(redirectUrl);
        }

        public object BeforeCall(string operationName, object[] inputs) 
        {
            return null;
        }

        // ────────────────────────── Private Members ──────────────────────────

        private string GetRedirectUrl(string redirectUrlQuerystringParameter)
        {
            if (redirectUrlQuerystringParameter != null)
            {
                UriTemplateMatch match = WebOperationContext.Current.IncomingRequest.UriTemplateMatch;
                if (match != null && match.QueryParameters.Count > 0)
                {
                    return match.QueryParameters[redirectUrlQuerystringParameter];
                }
            }
            return null;
        }
    }
}
