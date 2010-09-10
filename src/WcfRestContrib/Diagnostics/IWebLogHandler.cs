using System;
using WcfRestContrib.Net.Http;

namespace WcfRestContrib.Diagnostics
{
    public interface IWebLogHandler
    {
        void Write(Exception exception, RequestInformation information);
    }
}
