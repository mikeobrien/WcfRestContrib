using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
            MessageProperties incomingMessageProperties = context.IncomingMessageProperties;
            if (incomingMessageProperties != null)
            {
                SecurityMessageProperty security = 
                    incomingMessageProperties.Security ?? 
                    (incomingMessageProperties.Security = new SecurityMessageProperty());

                List<IAuthorizationPolicy> policies = security.ServiceSecurityContext.AuthorizationPolicies.ToList();
                policies.Add(new IdentityAuthorizationPolicy(identity));

                AuthorizationContext authorizationContext =
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
