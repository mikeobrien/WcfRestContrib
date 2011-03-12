using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.IdentityModel.Claims;

namespace WcfRestContrib.IdentityModel.Policy
{
    public class IdentityAuthorizationPolicy : IAuthorizationPolicy
    {
        private const string AuthContextIdentityPropertyName = "Identities";
        private readonly IIdentity _identity;

        public IdentityAuthorizationPolicy(IIdentity identity)
        {
            Id = Guid.NewGuid().ToString();
            _identity = identity;
        }

        public string Id
        { get; private set; }

        public ClaimSet Issuer
        { get { return ClaimSet.System; } }

        public bool Evaluate(EvaluationContext context, ref object state)
        {
            var identities = new List<IIdentity> {_identity};

            context.AddClaimSet(this, 
                new DefaultClaimSet(Issuer, new Claim(ClaimTypes.Name, _identity == null ? null : _identity.Name, Rights.Identity)));

            if (context.Properties.ContainsKey(AuthContextIdentityPropertyName))
                context.Properties[AuthContextIdentityPropertyName] = identities;
            else
                context.Properties.Add(AuthContextIdentityPropertyName, identities);

            return true;
        }
    }
}
