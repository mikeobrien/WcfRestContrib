using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfRestContrib.Net.Http;
using System.ServiceModel.Web;
using System.IdentityModel.Selectors;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.ServiceModel.Web.Exceptions;
using System.Security.Principal;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public class WebBasicAuthenticationHandler : IWebAuthenticationHandler 
    {
        public IIdentity Authenticate(
            IncomingWebRequestContext request, 
            OutgoingWebResponseContext response, 
            object[] parameters, 
            UserNamePasswordValidator validator,
            bool secure,
            bool requiresTransportLayerSecurity,
            string source)
        {
            if (requiresTransportLayerSecurity && !secure)
                throw new BasicRequiresTransportSecurityException(source);
            BasicAuthentication authentication = new BasicAuthentication(request.Headers);
            if (!authentication.Authenticate(validator))
                throw new BasicUnauthorizedException(source);
            return new GenericIdentity(authentication.Username, "WebBasicAuthenticationHandler");
        }
    }
}
