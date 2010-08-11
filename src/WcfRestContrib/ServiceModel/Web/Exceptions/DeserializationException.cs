using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class DeserializationException : WebException
    {
        public DeserializationException(Exception exception)
            : base(exception,
                System.Net.HttpStatusCode.BadRequest, 
                GetMessage(exception)) { }

        public DeserializationException(Exception exception, string message, params object[] args)
            : base(exception,
                System.Net.HttpStatusCode.BadRequest, 
                message,
                args) { }

        private static string GetMessage(Exception exception)
        {
            string message = "The request body could not be deserialized. {0}";
            Exception messageException = null;

            messageException = FindException<XmlException>(exception);
            if (messageException != null)
                return string.Format(message, messageException.Message);

            messageException = FindException<SerializationException>(exception);
            if (messageException != null)
                return string.Format(message, messageException.Message);

            return string.Format(message, 
                "Please check the formatting of your request body.");
        }

        private static T FindException<T>(Exception exception) where T : Exception 
        {
            Exception currentException = exception;

            do
            {
                if (currentException.GetType() == typeof(T)) return (T)currentException;
                currentException = currentException.InnerException;
            } while (currentException != null);

            return null;
        }
    }
}
