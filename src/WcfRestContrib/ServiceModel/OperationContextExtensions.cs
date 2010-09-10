using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IdentityModel.Policy;
using WcfRestContrib.IdentityModel.Policy;
using System.Collections.ObjectModel;

namespace WcfRestContrib.ServiceModel
{
    public static class OperationContextExtensions
    {
        public static void ReplacePrimaryIdentity(this OperationContext context, IIdentity identity)
        {
            var incomingMessageProperties = context.IncomingMessageProperties;
            if (incomingMessageProperties != null)
            {
                var security = 
                    incomingMessageProperties.Security ?? 
                    (incomingMessageProperties.Security = new SecurityMessageProperty());

                var policies = security.ServiceSecurityContext.AuthorizationPolicies.ToList();
                policies.Add(new IdentityAuthorizationPolicy(identity));

                var authorizationContext =
                    AuthorizationContext.CreateDefaultAuthorizationContext(policies);

                security.ServiceSecurityContext = new ServiceSecurityContext(
                    authorizationContext,
                    new ReadOnlyCollection<IAuthorizationPolicy>(policies));
            }
        }

        public static bool HasTransportLayerSecurity(this OperationContext context)
        {
            return context.RequestContext.RequestMessage.Headers.To.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }
    }
}
