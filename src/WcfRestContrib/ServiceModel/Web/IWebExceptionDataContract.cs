using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace WcfRestContrib.ServiceModel.Web
{
    public interface IWebExceptionDataContract
    {
        void Init(WebException exception);
    }
}
