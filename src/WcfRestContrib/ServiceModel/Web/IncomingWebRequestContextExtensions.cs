using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Web
{
    public static class IncomingWebRequestContextExtensions
    {
        public static string[] GetAcceptTypes(this IncomingWebRequestContext context)
        {
            if (context.Accept != null)
                return context.Accept.Split(new char[] { ',' });
            else
                return null;
        }
    }
}
