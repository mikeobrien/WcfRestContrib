using System;
using System.Collections.Specialized;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Channels
{
    public static class MessageExtensions
    {
        public static NameValueCollection GetRequestUriTemplateMatchVariables(this Message message)
        {
            const string uriTemplateMatchResultsKey = "UriTemplateMatchResults";

            // The UriTemplateMatch is stashed in the message propertes
            if (message.Properties.ContainsKey(uriTemplateMatchResultsKey))
            {
                var match = message.Properties[uriTemplateMatchResultsKey] as UriTemplateMatch;
                if (match != null) return match.BoundVariables;
            }

            return null;
        }

        public static void SetWebContentFormatProperty(this Message message, WebContentFormat format)
        {
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
                message.Properties.Remove(WebBodyFormatMessageProperty.Name);

            message.Properties.Add(WebBodyFormatMessageProperty.Name, 
                            new WebBodyFormatMessageProperty(format));
        }

        public static void SetHttpResponseProperty(this Message message, HttpResponseMessageProperty httpResponse)
        {
            if (message.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                message.Properties.Remove(HttpResponseMessageProperty.Name);

            message.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);
        }

        public static HttpResponseMessageProperty GetHttpResponseProperty(this Message message)
        {
            if (message.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                return (HttpResponseMessageProperty)message.Properties[HttpResponseMessageProperty.Name];
            return null;
        }

        public static void UpdateHttpProperty(this Message message)
        {
            var httpResponse = message.GetHttpResponseProperty();
            if (httpResponse == null)
            {
                httpResponse = new HttpResponseMessageProperty();
                message.SetHttpResponseProperty(httpResponse);
            }
            httpResponse.StatusCode = WebOperationContext.Current.OutgoingResponse.StatusCode;
            httpResponse.StatusDescription = WebOperationContext.Current.OutgoingResponse.StatusDescription;
            httpResponse.SuppressEntityBody = WebOperationContext.Current.OutgoingResponse.SuppressEntityBody;

            for (var index = 0; index < WebOperationContext.Current.OutgoingResponse.Headers.Count; index++)
                httpResponse.Headers[WebOperationContext.Current.OutgoingResponse.Headers.Keys[index]] =
                    WebOperationContext.Current.OutgoingResponse.Headers[index];
        }
    }
}
