using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Web
{
    public static class OutgoingWebResponseContextExtensions
    {
        public static void Redirect(this OutgoingWebResponseContext context, string url)
        {
            context.StatusCode = System.Net.HttpStatusCode.Moved;
            context.Location = url;
        }

        public static void SetFilename(this OutgoingWebResponseContext context, string filename)
        {
            SetFilename(context, filename, null);
        }

        public static void SetFilename(this OutgoingWebResponseContext context, string filename, string contentType)
        {
            if (contentType != null)
                context.ContentType = contentType;
            context.Headers.Add("Content-Disposition", "attachment; filename=" + filename);
        }
    }
}
