using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Description;
using System.Net;
using WcfRestContrib.ServiceModel.Web.Exceptions;
using WcfRestContrib.Xml;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class WebDispatchFormatter : IDispatchMessageFormatter
    {
        // ────────────────────────── Public Constants ──────────────────────────

        public enum FormatterDirection
        {
            Both,
            Incomming,
            Outgoing
        }

        public const string WebDispatcherFormatterProperty = "WebDispatcherFormatter";
        public const string WebDispatcherFormatterAccept = "WebDispatcherFormatterAccept";

        // ────────────────────────── Private Fields ──────────────────────────

        private static QueryStringConverter _queryStringConverter = new QueryStringConverter();

        private RequestMessagePartDescription[] _requestParameters;
        private Type _responseType;
        private IDispatchMessageFormatter _originalFormatter;
        private FormatterDirection _direction;
        private WebFormatterFactory _formatterFactory;

        // ────────────────────────── Constructors ──────────────────────────

        public WebDispatchFormatter(
            WebFormatterFactory formatterFactory,
            OperationDescription operationDescription,
            IDispatchMessageFormatter originalFormatter,
            FormatterDirection direction)
        {
            _formatterFactory = formatterFactory;
            _requestParameters = operationDescription.GetRequestMessageParts();
            _responseType = operationDescription.GetResponseType();
            _direction = direction;

            if (direction != FormatterDirection.Both)
                _originalFormatter = originalFormatter;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public Message Serialize(object result, Type responseType, string[] contentTypes)
        {
            return Serialize(_formatterFactory, result, responseType, contentTypes);
        }

        // ────────────────────────── IDispatchMessageFormatter Members ──────────────────────────

        public void DeserializeRequest(Message message, object[] parameters)
        {
            OperationContext.Current.OutgoingMessageProperties.Add(
                WebDispatchFormatter.WebDispatcherFormatterProperty, this);

            string[] accept = WebOperationContext.Current.IncomingRequest.GetAcceptTypes();
            if (accept != null)
                OperationContext.Current.OutgoingMessageProperties.Add(
                    WebDispatchFormatter.WebDispatcherFormatterAccept, accept);

            if (_direction == FormatterDirection.Both || _direction == FormatterDirection.Incomming)
            {
                Deserialize(
                    message,
                    _formatterFactory,
                    ref parameters,
                    _requestParameters,
                    WebOperationContext.Current.IncomingRequest.ContentType);
            }
            else _originalFormatter.DeserializeRequest(message, parameters);
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            if (_direction == FormatterDirection.Both || _direction == FormatterDirection.Outgoing)
            {
                Message message = null;
                if (_responseType != null && result != null)
                {
                    string[] accept = null;
                    if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(
                        WebDispatchFormatter.WebDispatcherFormatterAccept))
                        accept = (string[])OperationContext.Current.OutgoingMessageProperties[
                            WebDispatchFormatter.WebDispatcherFormatterAccept];

                    message = Serialize(
                        _formatterFactory,
                        result,
                        _responseType,
                        accept);
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = true;
                    message = Message.CreateMessage(MessageVersion.None, null, new object());
                }
                message.UpdateHttpProperty();
                return message;
            }
            else return _originalFormatter.SerializeReply(messageVersion, parameters, result);
        }

        // ────────────────────────── Private Members ──────────────────────────

        private void Deserialize(
            Message message, 
            WebFormatterFactory formatterFactory, 
            ref object[] parameters, 
            RequestMessagePartDescription[] requestParameters,
            string contentType)
        {
            // Grab the actual uri values, this will be null if there are none
            NameValueCollection uriParameters = message.GetRequestUriTemplateMatchVariables();

            // Loop through the parameters and set them based on their type
            for (int i = 0; i < parameters.Length; i++)
            {
                switch (requestParameters[i].PartType)
                {
                    // If its a path segment it will always be a string so just set it
                    case RequestMessagePartDescription.MessagePartType.PathSegment:
                        if (uriParameters != null)
                            parameters[i] = uriParameters[requestParameters[i].Name];
                        break;
                    // If its a querystring value it can be any primitive type so we need to
                    // perform a conversion.
                    case RequestMessagePartDescription.MessagePartType.Querystring:
                        if (uriParameters != null)
                        {
                            try
                            {
                                parameters[i] = _queryStringConverter.ConvertStringToValue(
                                        uriParameters[requestParameters[i].Name],
                                        requestParameters[i].Type);
                            }
                            catch (Exception exception)
                            {
                                throw new DeserializationException(exception,
                                    "The querystring parameter '{0}' must be of type '{1}'.",
                                    requestParameters[i].Alias,
                                    requestParameters[i].Type.GetXsdType());
                            }
                       }
                        break;
                    // If it's the entity body then we need to deserialize it
                    case RequestMessagePartDescription.MessagePartType.EntityBody:
                        if (message.IsEmpty) continue;
                        IWebFormatter formatter = formatterFactory.CreateFormatter(contentType);
                        using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
                        {
                            WebFormatterDeserializationContext deserializationContext;

                            if (reader.Name == BinaryBodyReader.BINARY_ELEMENT_NAME)
                                deserializationContext = WebFormatterDeserializationContext.CreateBinary(new BinaryBodyReader(reader).Data);
                            else
                                deserializationContext = WebFormatterDeserializationContext.CreateXml(reader);

                            try
                            {
                                parameters[i] = formatter.Deserialize(deserializationContext, requestParameters[i].Type);
                            }
                            catch (Exception exception)
                            {
                                throw new DeserializationException(exception);
                            }
                        }
                        break;
                }
            }
        }

        private Message Serialize(
            WebFormatterFactory formatterFactory, 
            object result, 
            Type responseType, 
            string[] contentTypes)
        {
            string resolvedContentType;

            IWebFormatter formatter = formatterFactory.CreateFormatter(contentTypes, out resolvedContentType);
            WebFormatterSerializationContext serializationContext = formatter.Serialize(result, responseType);

            Message message = null;

            switch (serializationContext.ContentFormat)
            {
                case WebFormatterSerializationContext.SerializationFormat.Xml:
                    message = Message.CreateMessage(MessageVersion.None, null, result, serializationContext.XmlSerializer);
                    message.SetWebContentFormatProperty(WebContentFormat.Xml);
                    break;
                case WebFormatterSerializationContext.SerializationFormat.Json:
                    message = Message.CreateMessage(MessageVersion.None, null, result, serializationContext.XmlSerializer);
                    message.SetWebContentFormatProperty(WebContentFormat.Json);
                    break;
                case WebFormatterSerializationContext.SerializationFormat.Binary:
                    message = Message.CreateMessage(MessageVersion.None, null, new BinaryBodyWriter(serializationContext.BinaryData));
                    message.SetWebContentFormatProperty(WebContentFormat.Raw);
                    break;
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = resolvedContentType;

            return message;
        }
    }
}