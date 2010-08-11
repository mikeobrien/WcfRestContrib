using System;
using System.Linq;
using System.ServiceModel.Web;
using NielsBohrLibrary.Contracts;
using NielsBohrLibrary.Domain;
using WcfRestContrib.ServiceModel.Web;
using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace NielsBohrLibrary
{
    [ServiceConfiguration("Rest", true)]
    public class Books : IBooksService
    {
        public Contracts.Books GetBooks()
        {
            return new Contracts.Books(Catalog.GetCatalog().Select(
                i => new Book()
                {
                    Author = i.Author,
                    Description = i.Description,
                    Isbn = i.Isbn,
                    Language = i.Language,
                    Published = i.Published,
                    Summary = i.Summary,
                    Title = i.Title
                }));
        }

        public Contracts.Books GetBooksByYear(string year, string language)
        {
            int integerYear;
            if (!int.TryParse(year, out integerYear))
                throw new WebException(
                    System.Net.HttpStatusCode.BadRequest,
                    "Year is not valid.");

            return new Contracts.Books(Catalog.GetCatalog().
                Where(i => i.Published == integerYear).Select(
                i => new Book()
                {
                    Author = i.Author,
                    Description = i.Description,
                    Isbn = i.Isbn,
                    Language = i.Language,
                    Published = i.Published,
                    Summary = i.Summary,
                    Title = i.Title
                }));
        }

        public Book GetBook(string isbn)
        {
            CatalogItem item = Catalog.GetItem(isbn);
            if (item == null)
                throw new WebException(System.Net.HttpStatusCode.NotFound,
                    "Sorry, we dont have the book you're looking for.");
            
            return new Book()
            {
               Author = item.Author,
               Description = item.Description,
               Isbn = item.Isbn,
               Language = item.Language,
               Published = item.Published,
               Summary = item.Summary,
               Title = item.Title
            };
        }

        public void BreakMe()
        {
            throw new Exception("Bad things are happening...");
        }

        public void AddBook(Book book, string redirectUrl)
        {
            if (book == null) throw new ArgumentNullException("book");
            AddOrModifyBook(book.Isbn, book);
        }

        public void AddOrModifyBook(string isbn, Book book)
        {
            if (book == null) throw new ArgumentNullException("book");
            if (Catalog.ItemExists(isbn))
                Catalog.UpdateItem(isbn,
                    new CatalogItem()
                    {
                        Author = book.Author,
                        Description = book.Description,
                        Isbn = book.Isbn,
                        Language = book.Language,
                        Published = book.Published,
                        Summary = book.Summary,
                        Title = book.Title
                    });
            else
            {
                book.Isbn = isbn;
                Catalog.AddItem(
                    new CatalogItem()
                    {
                        Author = book.Author,
                        Description = book.Description,
                        Isbn = book.Isbn,
                        Language = book.Language,
                        Published = book.Published,
                        Summary = book.Summary,
                        Title = book.Title
                    });
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
            }
        }

        public void DeleteBook(string isbn)
        {
            Catalog.DeleteItem(isbn);
        }
    }
}
