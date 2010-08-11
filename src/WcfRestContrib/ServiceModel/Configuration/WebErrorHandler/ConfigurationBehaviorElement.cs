using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Configuration;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.Reflection;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.Diagnostics;

namespace WcfRestContrib.ServiceModel.Configuration.WebErrorHandler
{
    public class ConfigurationBehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string LOG_HANDLER_TYPE_ELEMENT = "logHandlerType";
        private const string UNHANDLED_ERROR_MESSAGE_ELEMENT = "unhandledErrorMessage";
        private const string RETURN_RAW_EXCEPTION = "returnRawException";
        private const string EXCEPTION_DATA_CONTRACT_TYPE_ELEMENT = "exceptionDataContractType";

        // ────────────────────────── BehaviorExtensionElement Overrides ──────────────────────────

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

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(LOG_HANDLER_TYPE_ELEMENT, IsRequired = false, DefaultValue = null)]
        public string LogHandlerType
        {
            get
            { return (string)base[LOG_HANDLER_TYPE_ELEMENT]; }
            set
            { base[LOG_HANDLER_TYPE_ELEMENT] = value; }
        }

        [ConfigurationProperty(UNHANDLED_ERROR_MESSAGE_ELEMENT, IsRequired = false, DefaultValue = null)]
        public string UnhandledErrorMessage
        {
            get
            { return (string)base[UNHANDLED_ERROR_MESSAGE_ELEMENT]; }
            set
            { base[UNHANDLED_ERROR_MESSAGE_ELEMENT] = value; }
        }

        [ConfigurationProperty(RETURN_RAW_EXCEPTION, IsRequired = false, DefaultValue = false)]
        public bool ReturnRawException
        {
            get
            { return (bool)base[RETURN_RAW_EXCEPTION]; }
            set
            { base[RETURN_RAW_EXCEPTION] = value; }
        }

        [ConfigurationProperty(EXCEPTION_DATA_CONTRACT_TYPE_ELEMENT, IsRequired = false, DefaultValue = null)]
        public string ExceptionDataContractType
        {
            get
            { return (string)base[EXCEPTION_DATA_CONTRACT_TYPE_ELEMENT]; }
            set
            { base[EXCEPTION_DATA_CONTRACT_TYPE_ELEMENT] = value; }
        }
    }
}
