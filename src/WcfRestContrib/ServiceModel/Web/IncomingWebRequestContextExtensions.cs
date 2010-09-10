using System.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Web
{
    public static class IncomingWebRequestContextExtensions
    {
        public static string[] GetAcceptTypes(this IncomingWebRequestContext context)
        {
            return context.Accept != null ? context.Accept.Split(new [] { ',' }) : null;
        }
    }
}
