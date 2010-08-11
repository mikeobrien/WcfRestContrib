using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfRestContrib.ServiceModel.Web.Exceptions;

namespace WcfRestContrib.ServiceModel.Web
{
    public interface IWebExceptionDataContract
    {
        void Init(WebException exception);
    }
}
