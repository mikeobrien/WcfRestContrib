using WcfRestContrib.Net.Http;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class BasicUnauthorizedException : WebException 
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private readonly string _realm;

        // ────────────────────────── Constructors ──────────────────────────

        public BasicUnauthorizedException(string realm)
            : base(
                System.Net.HttpStatusCode.Unauthorized,
                "You have unsuccessfully attempted to access a secure resource.")
        {
            _realm = realm;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public override void UpdateHeaders(System.Net.WebHeaderCollection headers)
        {
            BasicAuthentication.SetUnauthorizedHeader(headers, _realm);
        }
    }
}
