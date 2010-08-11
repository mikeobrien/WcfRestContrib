using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ServiceModel.Channels;
using System.Net;
using System.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Channels
{
    public static class MessageExtensions
    {
        /// <summary>
        /// Gets the UriTemplateMatch BoundVariables from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static NameValueCollection GetRequestUriTemplateMatchVariables(this Message message)
        {
            string UriTemplateMatchResultsKey = "UriTemplateMatchResults";

            // The UriTemplateMatch is stashed in the message propertes
            if (message.Properties.ContainsKey(UriTemplateMatchResultsKey))
            {
                UriTemplateMatch match = message.Properties[UriTemplateMatchResultsKey] as UriTemplateMatch;
                if (match != null) return match.BoundVariables;
            }

            return null;
        }

        /// <summary>
        /// Sets the web content format property. Removes existing one if it exists.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="format"></param>
        public static void SetWebContentFormatProperty(this Message message, WebContentFormat format)
        {
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
                message.Properties.Remove(WebBodyFormatMessageProperty.Name);

            message.Properties.Add(WebBodyFormatMessageProperty.Name, 
                            new WebBodyFormatMessageProperty(format));
        }

        /// <summary>
        /// Sets the http response information. Removes existing one if it exists.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="httpResponse"></param>
        public static void SetHttpResponseProperty(this Message message, HttpResponseMessageProperty httpResponse)
        {
            if (message.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                message.Properties.Remove(HttpResponseMessageProperty.Name);

            message.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);
        }

        /// <summary>
        /// Gets the http response information. Removes existing one if it exists.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="httpResponse"></param>
        public static HttpResponseMessageProperty GetHttpResponseProperty(this Message message)
        {
            if (message.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                return (HttpResponseMessageProperty)message.Properties[HttpResponseMessageProperty.Name];
            else
                return null;
        }

        public static void UpdateHttpProperty(this Message message)
        {
            HttpResponseMessageProperty httpResponse = message.GetHttpResponseProperty();
            if (httpResponse == null)
            {
                httpResponse = new HttpResponseMessageProperty();
                message.SetHttpResponseProperty(httpResponse);
            }
            httpResponse.StatusCode = WebOperationContext.Current.OutgoingResponse.StatusCode;
            httpResponse.StatusDescription = WebOperationContext.Current.OutgoingResponse.StatusDescription;
            httpResponse.SuppressEntityBody = WebOperationContext.Current.OutgoingResponse.SuppressEntityBody;

            for (int index = 0; index < WebOperationContext.Current.OutgoingResponse.Headers.Count; index++)
                httpResponse.Headers[WebOperationContext.Current.OutgoingResponse.Headers.Keys[index]] =
                    WebOperationContext.Current.OutgoingResponse.Headers[index];
        }
    }
}
