using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using WcfRestContrib.ServiceModel;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Dispatcher;
using WcfRestContrib.Net.Http;
using WcfRestContrib.ServiceModel.Web;

namespace NielsBohrLibrary.Contracts
{
    [ServiceContract]
    //[ServiceAuthentication]
    public interface IBooksService
    {
        [WebGet(UriTemplate = "/")]
        [WebDispatchFormatter]
        [OperationContract]
        [OperationAuthentication]
        Books GetBooks();

        [WebGet(UriTemplate = "/breakme")]
        [WebDispatchFormatter]
        [OperationContract]
        void BreakMe();

        [WebGet(UriTemplate = "/published/{year}?language={language}")]
        [WebDispatchFormatter]
        [OperationContract]
        Books GetBooksByYear(string year, string language);

        [WebInvoke(UriTemplate = "/?redirecturl={redirectUrl}", Method=Verbs.Post)]
        [WebDispatchFormatter]
        [OperationContract]
        [OperationAuthentication]
        [Redirect("redirecturl")]
        void AddBook(Book book, string redirectUrl);

        [WebGet(UriTemplate = "/{isbn}")]
        [WebDispatchFormatter]
        [OperationContract]
        Book GetBook(string isbn);

        [WebInvoke(UriTemplate = "/{isbn}", Method=Verbs.Put)]
        [WebDispatchFormatter]
        [OperationContract]
        [OperationAuthentication]
        void AddOrModifyBook(string isbn, Book book);

        [WebInvoke(UriTemplate = "/{isbn}", Method=Verbs.Delete)]
        [WebDispatchFormatter]
        [OperationContract]
        [OperationAuthentication]
        void DeleteBook(string isbn);
    }
}
