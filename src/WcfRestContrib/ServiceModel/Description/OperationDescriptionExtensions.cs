using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Web;
using WcfRestContrib.Collections.Specialized;

namespace WcfRestContrib.ServiceModel.Description
{
    public static class OperationDescriptionExtensions
    {
        public static Type GetResponseType(this OperationDescription operationDescription)
        {
            var message = operationDescription.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Output);
            return message != null ? message.Body.ReturnValue.Type : null;
        }

        public static RequestMessagePartDescription[] GetRequestMessageParts(this OperationDescription operationDescription)
        {
            var message = operationDescription.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Input);
            if (message == null) return null;

            var parameters = new List<RequestMessagePartDescription>();
            var uriTemplate = operationDescription.GetWebUriTemplate();
            var uriTemplateQuerystring = operationDescription.GetWebUriTemplateQuerystring();
            List<string> pathSegments = null;
            List<string> queryValues = null;

            if (uriTemplate != null)
            {
                pathSegments = new List<string>(uriTemplate.PathSegmentVariableNames);
                queryValues = new List<string>(uriTemplate.QueryValueVariableNames);
            }

            foreach (var part in message.Body.Parts)
            {
                RequestMessagePartDescription.MessagePartType type;
                string alias = null;
                if (pathSegments != null &&
                    pathSegments.FirstOrDefault(p => p.Equals(part.Name, StringComparison.OrdinalIgnoreCase)) !=
                    null)
                    type = RequestMessagePartDescription.MessagePartType.PathSegment;
                else if (queryValues != null &&
                         queryValues.FirstOrDefault(p => p.Equals(part.Name, StringComparison.OrdinalIgnoreCase)) !=
                         null)
                {
                    type = RequestMessagePartDescription.MessagePartType.Querystring;
                    var querystringPart = uriTemplateQuerystring.FirstOrDefault(
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

        public static UriTemplate GetWebUriTemplate(this OperationDescription operationDescription)
        {
            var template = GetRawWebUriTemplate(operationDescription);
            return !string.IsNullOrEmpty(template) ? new UriTemplate(template) : null;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetWebUriTemplateQuerystring(this OperationDescription operationDescription)
        {
            var template = GetRawWebUriTemplate(operationDescription);
            return !string.IsNullOrEmpty(template) ? HttpUtility.ParseQueryString(new Uri(new Uri("dummy:"), template).Query).ToEnumerable() : null;
        }

        public static string GetRawWebUriTemplate(this OperationDescription operationDescription)
        {
            var webGetAttribute = operationDescription.Behaviors.Find<WebGetAttribute>();
            if (webGetAttribute != null)
            {
                if (webGetAttribute.UriTemplate != null) return webGetAttribute.UriTemplate;
            }
            else
            {
                var webInvokeAttribute = operationDescription.Behaviors.Find<WebInvokeAttribute>();
                if (webInvokeAttribute != null && webInvokeAttribute.UriTemplate != null)
                    return webInvokeAttribute.UriTemplate;
            }
            return null;
        }
    }
}
