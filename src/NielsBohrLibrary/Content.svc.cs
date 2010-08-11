using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NielsBohrLibrary.Contracts;
using WcfRestContrib.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Web;
using WcfRestContrib.IO;
using System.Net.Mime;
using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace NielsBohrLibrary
{
    [ServiceConfiguration("Rest", true, TransferMode.Streamed, "HttpStreamedRest", "HttpsStreamedRest")]
    public class Content : IContentService
    {
        public void Upload(Stream bookContent, string isbn)
        {
            if (WebOperationContext.Current.IncomingRequest.ContentType == MediaTypeNames.Application.Pdf)
            {
                if (!Domain.Catalog.SaveContentStream(bookContent, isbn))
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
            }
            else
                throw new WebException(
                    System.Net.HttpStatusCode.UnsupportedMediaType,
                    "'{0}' is not a supported media type. This service only accepts '{1}'.",
                    WebOperationContext.Current.IncomingRequest.ContentType,
                    MediaTypeNames.Application.Pdf);
        }

        public Stream Download(string isbn)
        {
            Stream contentStream;
            string filename, contentType;

            if (Domain.Catalog.TryGetContentStream(isbn, out filename, out contentType, out contentStream))
            {
                WebOperationContext.Current.OutgoingResponse.SetFilename(filename);
                WebOperationContext.Current.OutgoingResponse.ContentType = contentType;

                return contentStream;
            }
            else
                throw new WebException(
                    System.Net.HttpStatusCode.NotFound,
                    "Book content is not available for ISBN {0}.", isbn);
        }
    }
}
