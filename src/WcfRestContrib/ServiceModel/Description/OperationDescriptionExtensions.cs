using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Web;
using System.Collections.Specialized;
using WcfRestContrib.Collections.Specialized;

namespace WcfRestContrib.ServiceModel.Description
{
    public static class OperationDescriptionExtensions
    {
        /// <summary>
        /// Gets the return type of the outgoing message.
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <returns></returns>
        public static Type GetResponseType(this OperationDescription operationDescription)
        {
            MessageDescription message = operationDescription.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Output);
            if (message != null)
                return message.Body.ReturnValue.Type;
            else
                return null;
        }

        /// <summary>
        /// Returns an array of message part descriptions. These include the part type (IE: EntityBody, PathSegment and Querystring).
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <returns></returns>
        public static RequestMessagePartDescription[] GetRequestMessageParts(this OperationDescription operationDescription)
        {
            MessageDescription message = operationDescription.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Input);
            if (message != null)
            {
                List<RequestMessagePartDescription> parameters = new List<RequestMessagePartDescription>();
                UriTemplate uriTemplate = operationDescription.GetWebUriTemplate();
                IEnumerable<KeyValuePair<string, string>> uriTemplateQuerystring = operationDescription.GetWebUriTemplateQuerystring();
                List<string> pathSegments = null;
                List<string> queryValues = null;

                if (uriTemplate != null)
                {
                    pathSegments = new List<string>(uriTemplate.PathSegmentVariableNames);
                    queryValues = new List<string>(uriTemplate.QueryValueVariableNames);
                }

                foreach (System.ServiceModel.Description.MessagePartDescription part in message.Body.Parts)
                {
                    RequestMessagePartDescription.MessagePartType type;
                    string alias = null;
                    if (pathSegments != null &&
                        pathSegments.FirstOrDefault(p => p.Equals(part.Name, StringComparison.OrdinalIgnoreCase)) != null)
                        type = RequestMessagePartDescription.MessagePartType.PathSegment;
                    else if (queryValues != null &&
                        queryValues.FirstOrDefault(p => p.Equals(part.Name, StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        type = RequestMessagePartDescription.MessagePartType.Querystring;
                        KeyValuePair<string, string> querystringPart = uriTemplateQuerystring.FirstOrDefault(
                            p => p.Value == string.Format("{{{0}}}", part.Name));
                        if (!string.IsNullOrEmpty(querystringPart.Key)) alias = querystringPart.Key;
                    }
                    else
                        type = RequestMessagePartDescription.MessagePartType.EntityBody;
                    if (string.IsNullOrEmpty(alias)) alias = part.Name;
                    parameters.Add(new RequestMessagePartDescription(part, type, alias));
                }

                return parameters.OrderBy(p => p.Index).ToArray();
            }
            else
                return null;    
        }

        /// <summary>
        /// Gets the UriTemplate from either the WebGet or WebInvoke behavior.
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <returns></returns>
        public static UriTemplate GetWebUriTemplate(this OperationDescription operationDescription)
        {
            string template = GetRawWebUriTemplate(operationDescription);
            if (!string.IsNullOrEmpty(template))
                return new UriTemplate(template);
            else
                return null;
        }

        /// <summary>
        /// Gets the UriTemplate querystring from either the WebGet or WebInvoke behavior.
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetWebUriTemplateQuerystring(this OperationDescription operationDescription)
        {
            WebGetAttribute webGetAttribute = operationDescription.Behaviors.Find<WebGetAttribute>();
            string template = GetRawWebUriTemplate(operationDescription);
            if (!string.IsNullOrEmpty(template))
                return HttpUtility.ParseQueryString(new Uri(new Uri("dummy:"), template).Query).ToEnumerable();
            else
                return null;
        }

                /// <summary>
        /// Gets the UriTemplate from either the WebGet or WebInvoke behavior.
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <returns></returns>
        public static string GetRawWebUriTemplate(this OperationDescription operationDescription)
        {
            WebGetAttribute webGetAttribute = operationDescription.Behaviors.Find<WebGetAttribute>();
            if (webGetAttribute != null)
            {
                if (webGetAttribute.UriTemplate != null) return webGetAttribute.UriTemplate;
            }
            else
            {
                WebInvokeAttribute webInvokeAttribute = operationDescription.Behaviors.Find<WebInvokeAttribute>();
                if (webInvokeAttribute != null && webInvokeAttribute.UriTemplate != null)
                    return webInvokeAttribute.UriTemplate;
            }
            return null;
        }
    }
}
