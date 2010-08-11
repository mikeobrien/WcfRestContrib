using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfRestContrib.ServiceModel.Web.Exceptions
{
    public class InvalidParameterException : WebException
    {
        public InvalidParameterException(string name, string value)
            : base(
                System.Net.HttpStatusCode.BadRequest,
                "{0} '{1}' is not valid.", name, 
                value.Ellipsis(50, "[NULL]", "[EMPTY]")) { }
    }
}
