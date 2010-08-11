using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using WcfRestContrib.ServiceModel.Web;

namespace WcfRestContrib.ServiceModel.Activation
{
    public class WebServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new WcfRestContrib.ServiceModel.Web.WebServiceHost(serviceType, baseAddresses);
        }
    }
}