using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Net;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class RedirectInspector : Attribute, IParameterInspector 
    {
        private readonly string _redirectUrlQuerystringParameter;

        public RedirectInspector(
            string redirectUrlQuerystringParameter)
        {
            _redirectUrlQuerystringParameter = redirectUrlQuerystringParameter;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            // Check to see if there is a redirect parameter set in the attribute 
            // and that its specified in the uri
            var redirectUrl = GetRedirectUrl(_redirectUrlQuerystringParameter);
            if (redirectUrl != null && redirectUrl.Trim().Length > 0 &&
                WebOperationContext.Current.OutgoingResponse.StatusCode == HttpStatusCode.OK) 
                WebOperationContext.Current.OutgoingResponse.Redirect(redirectUrl);
        }

        public object BeforeCall(string operationName, object[] inputs) 
        {
            return null;
        }

        private static string GetRedirectUrl(string redirectUrlQuerystringParameter)
        {
            if (redirectUrlQuerystringParameter != null)
            {
                var match = WebOperationContext.Current.IncomingRequest.UriTemplateMatch;
                if (match != null && match.QueryParameters.Count > 0)
                {
                    return match.QueryParameters[redirectUrlQuerystringParameter];
                }
            }
            return null;
        }
    }
}
