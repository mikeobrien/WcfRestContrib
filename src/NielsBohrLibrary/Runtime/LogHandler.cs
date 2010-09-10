using System;
using WcfRestContrib.Diagnostics;
using System.Diagnostics;
using WcfRestContrib.Net.Http;
using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace NielsBohrLibrary.Runtime
{
    public class LogHandler : IWebLogHandler 
    {
        public void Write(Exception exception, RequestInformation info)
        {
            if (!(exception is WebException))
                Debug.WriteLine(
                    string.Format("{0} - {1}: {2}",
                    info.Method,
                    info.Uri.ToString(),
                    exception.Message));
        }
    }
}
