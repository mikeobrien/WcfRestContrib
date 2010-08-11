using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WcfRestContrib.Net.Http;

namespace WcfRestContrib.Diagnostics
{
    public interface IWebLogHandler
    {
        void Write(Exception exception, RequestInformation information);
    }
}
