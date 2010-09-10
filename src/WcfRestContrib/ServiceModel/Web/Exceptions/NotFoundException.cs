namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class NotFoundException : WebException
    {
        public NotFoundException(string message, params object[] args)
            : base(
                System.Net.HttpStatusCode.NotFound, message, args) { }
    }
}
