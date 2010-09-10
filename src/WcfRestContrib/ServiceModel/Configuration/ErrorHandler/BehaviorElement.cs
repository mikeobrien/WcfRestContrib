using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;
using System.Configuration;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration.ErrorHandler
{
    public class BehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── BehaviorExtensionElement Overrides ──────────────────────────

        private const string ErrorHandlerTypeElement = "errorHandlerType";
        private const string UnhandledErrorMessageElement = "unhandledErrorMessage";
        private const string ReturnRawExceptionElement = "returnRawException";

        // ────────────────────────── BehaviorExtensionElement Overrides ──────────────────────────

        public override Type BehaviorType
        {
            get { return typeof (ErrorHandlerBehavior); }
        }

        protected override object CreateBehavior()
        {
            Type errorHandler;
            try
            {
                errorHandler = ErrorHandlerType.GetType<IErrorHandler>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Invalid errorHandlerType specified in errorHandler behavior element. {0}", e));
            }

            return new ErrorHandlerBehavior(
                errorHandler,
                UnhandledErrorMessage,
                ReturnRawException);
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(ErrorHandlerTypeElement, IsRequired = true)]
        public string ErrorHandlerType
        {
            get { return (string) base[ErrorHandlerTypeElement]; }
            set { base[ErrorHandlerTypeElement] = value; }
        }

        [ConfigurationProperty(UnhandledErrorMessageElement, IsRequired = false, DefaultValue = null)]
        public string UnhandledErrorMessage
        {
            get
            { return (string)base[UnhandledErrorMessageElement]; }
            set
            { base[UnhandledErrorMessageElement] = value; }
        }

        [ConfigurationProperty(ReturnRawExceptionElement, IsRequired = false, DefaultValue = false)]
        public bool ReturnRawException
        {
            get
            { return (bool)base[ReturnRawExceptionElement]; }
            set
            { base[ReturnRawExceptionElement] = value; }
        }
    }
}