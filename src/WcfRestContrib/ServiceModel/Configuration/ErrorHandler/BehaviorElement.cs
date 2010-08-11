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

namespace WcfRestContrib.ServiceModel.Configuration.ErrorHandler
{
    public class BehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── BehaviorExtensionElement Overrides ──────────────────────────

        private const string ELEMENT_ERROR_HANDLER_TYPE = "errorHandlerType";

        // ────────────────────────── BehaviorExtensionElement Overrides ──────────────────────────

        public override Type BehaviorType
        {
            get { return typeof (ErrorHandlerBehavior); }
        }

        protected override object CreateBehavior()
        {
            Type errorHandler = null;
            try
            {
                errorHandler = ErrorHandlerType.GetType<IErrorHandler>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Invalid errorHandlerType specified in errorHandler behavior element. {0}", e));
            }

            return new ErrorHandlerBehavior(errorHandler);
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(ELEMENT_ERROR_HANDLER_TYPE, IsRequired = true)]
        public string ErrorHandlerType
        {
            get { return (string) base[ELEMENT_ERROR_HANDLER_TYPE]; }
            set { base[ELEMENT_ERROR_HANDLER_TYPE] = value; }
        }
    }
}