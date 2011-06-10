using System;
using System.ServiceModel.Configuration;
using System.Configuration;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.Reflection;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.Diagnostics;

namespace WcfRestContrib.ServiceModel.Configuration.WebErrorHandler
{
    public class BehaviorElement : BehaviorExtensionElement
    {
        private const string LogHandlerTypeElement = "logHandlerType";
        private const string UnhandledErrorMessageElement = "unhandledErrorMessage";
        private const string ReturnRawExceptionElement = "returnRawException";
        private const string ExceptionDataContractTypeElement = "exceptionDataContractType";

        public override Type BehaviorType
        {
            get { return typeof(WebErrorHandlerConfigurationBehavior); }
        }

        protected override object CreateBehavior()
        {
            Type logHandler = null;
            if (!string.IsNullOrEmpty(LogHandlerType))
                try
                {
                    logHandler = LogHandlerType.GetType<IWebLogHandler>();
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Invalid logHandlerType specified in webErrorHandler behavior element. {0}", e));
                }

            Type exceptionDataContract = null;
            if (!string.IsNullOrEmpty(ExceptionDataContractType))
                try
                {
                    exceptionDataContract = ExceptionDataContractType.GetType<IWebExceptionDataContract>();
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Invalid exceptionDataContractType specified in webErrorHandler behavior element. {0}", e));
                }
           
            return new WebErrorHandlerConfigurationBehavior(
                logHandler,
                UnhandledErrorMessage,
                ReturnRawException,
                exceptionDataContract);
        }

        [ConfigurationProperty(LogHandlerTypeElement, IsRequired = false, DefaultValue = null)]
        public string LogHandlerType
        {
            get
            { return (string)base[LogHandlerTypeElement]; }
            set
            { base[LogHandlerTypeElement] = value; }
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

        [ConfigurationProperty(ExceptionDataContractTypeElement, IsRequired = false, DefaultValue = null)]
        public string ExceptionDataContractType
        {
            get
            { return (string)base[ExceptionDataContractTypeElement]; }
            set
            { base[ExceptionDataContractTypeElement] = value; }
        }
    }
}
