namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class BasicRequiresTransportSecurityException : WebException 
    {
        public BasicRequiresTransportSecurityException() : base(
                System.Net.HttpStatusCode.Forbidden,
                "This resource must be accessed over SSL/TLS.") { }
    }
}
