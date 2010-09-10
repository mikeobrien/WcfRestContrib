using System.IdentityModel.Selectors;
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
            UserNamePasswordValidator validator,
            bool secure,
            bool requiresTransportLayerSecurity,
            string source);
    }
}
