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
using WcfRestContrib.ServiceModel.Dispatcher;
using System.IdentityModel.Selectors;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration.WebAuthentication
{
    public class ConfigurationBehaviorElement : BehaviorExtensionElement
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string AUTH_HANDLER_TYPE_ELEMENT = "authenticationHandlerType";
        private const string USERNAME_PASSWORD_VALIDATOR_TYPE_ELEMENT = "usernamePasswordValidatorType";
        private const string REQUIRE_SECURE_TRANSPORT_ELEMENT = "requireSecureTransport";
        private const string SOURCE_ELEMENT = "source";

        // ────────────────────────── BehaviorExtensionElement Overrides ────────────────

        public override Type BehaviorType
        {
            get { return typeof(WebAuthenticationConfigurationBehavior); }
        }

        protected override object CreateBehavior()
        {
            Type operationAuthenticationHandler;
            try
            {
                operationAuthenticationHandler = OperationAuthenticationHandlerTypeName.GetType<IWebAuthenticationHandler>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Invalid authenticationHandlerType specified in webAuthentication behavior element. {0}", e));
            }

            Type usernamePasswordValidator;
            try
            {
                usernamePasswordValidator = UsernamePasswordValidatorTypeName.GetType<UserNamePasswordValidator>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Invalid usernamePasswordValidatorType specified in webAuthentication behavior element. {0}", e));
            }
         
            return new WebAuthenticationConfigurationBehavior(
                operationAuthenticationHandler,
                usernamePasswordValidator,
                RequireSecureTransport,
                Source);
        }

        // ────────────────────────── Public Members ──────────────────────────

        [ConfigurationProperty(AUTH_HANDLER_TYPE_ELEMENT, IsRequired = true)]
        public string OperationAuthenticationHandlerTypeName
        {
            get
            { return (string)base[AUTH_HANDLER_TYPE_ELEMENT]; }
            set
            { base[AUTH_HANDLER_TYPE_ELEMENT] = value; }
        }

        [ConfigurationProperty(USERNAME_PASSWORD_VALIDATOR_TYPE_ELEMENT, IsRequired = true)]
        public string UsernamePasswordValidatorTypeName
        {
            get
            { return (string)base[USERNAME_PASSWORD_VALIDATOR_TYPE_ELEMENT]; }
            set
            { base[USERNAME_PASSWORD_VALIDATOR_TYPE_ELEMENT] = value; }
        }

        [ConfigurationProperty(REQUIRE_SECURE_TRANSPORT_ELEMENT, IsRequired = false, DefaultValue = true)]
        public bool RequireSecureTransport
        {
            get
            { return (bool)base[REQUIRE_SECURE_TRANSPORT_ELEMENT]; }
            set
            { base[REQUIRE_SECURE_TRANSPORT_ELEMENT] = value; }
        }

        [ConfigurationProperty(SOURCE_ELEMENT, IsRequired = true)]
        public string Source
        {
            get
            { return (string)base[SOURCE_ELEMENT]; }
            set
            { base[SOURCE_ELEMENT] = value; }
        }
    }
}
