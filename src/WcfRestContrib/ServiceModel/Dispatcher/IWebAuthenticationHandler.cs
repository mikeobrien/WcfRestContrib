using System;
using System.ServiceModel.Web;
using System.Security.Principal;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public interface IWebAuthenticationHandler
    {
        IIdentity Authenticate(
            IncomingWebRequestContext request, 
            OutgoingWebResponseContext response, 
            object[] parameters,
            Type validatorType,
            bool secure,
            bool requiresTransportLayerSecurity,
            string source);
    }
}
