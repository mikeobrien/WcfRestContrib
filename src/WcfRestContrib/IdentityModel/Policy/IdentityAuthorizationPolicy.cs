using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.IdentityModel.Claims;

namespace WcfRestContrib.IdentityModel.Policy
{
    public class IdentityAuthorizationPolicy : IAuthorizationPolicy
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private const string AUTH_CONTEXT_IDENTITY_PROPERTY_NAME = "Identities";
        private IIdentity _identity;

        // ────────────────────────── Constructors ──────────────────────────

        public IdentityAuthorizationPolicy(IIdentity identity)
        {
            Id = Guid.NewGuid().ToString();
            _identity = identity;
        }

        // ────────────────────────── IAuthorizationPolicy Implementation ──────────────────────────

        public string Id
        { get; private set; }

        public ClaimSet Issuer
        { get { return ClaimSet.System; } }

        public bool Evaluate(EvaluationContext context, ref object state)
        {
            List<IIdentity> identities = new List<IIdentity>();
            identities.Add(_identity);

            context.AddClaimSet(this, 
                new DefaultClaimSet(this.Issuer, new Claim(ClaimTypes.Name, _identity == null ? null : _identity.Name, Rights.Identity)));

            if (context.Properties.ContainsKey(AUTH_CONTEXT_IDENTITY_PROPERTY_NAME))
                context.Properties[AUTH_CONTEXT_IDENTITY_PROPERTY_NAME] = identities;
            else
                context.Properties.Add(AUTH_CONTEXT_IDENTITY_PROPERTY_NAME, identities);

            return true;
        }
    }
}
