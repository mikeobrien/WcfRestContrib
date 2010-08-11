using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class NotFoundException : WebException
    {
        public NotFoundException(string message, params object[] args)
            : base(
                System.Net.HttpStatusCode.NotFound, message, args) { }
    }
}
