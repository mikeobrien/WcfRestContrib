using System.ServiceModel;
using System.ServiceModel.Web;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.Net.Http;
using System.IO;

namespace NielsBohrLibrary.Contracts
{
    [ServiceContract]
    public interface IContentService
    {
        [WebGet(UriTemplate = "/{isbn}")]
        [OperationContract]
        Stream Download(string isbn);

        [WebInvoke(UriTemplate = "/{isbn}", Method=Verbs.Post)]
        [WebDispatchFormatter(WebDispatchFormatter.FormatterDirection.Outgoing)]
        [OperationContract]
        [OperationAuthentication]
        void Upload(Stream content, string isbn);
    }
}
