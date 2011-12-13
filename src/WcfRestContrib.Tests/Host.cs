using System;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using RestSharp;

namespace WcfRestContrib.Tests
{
    public class Host<TService> : IDisposable
    {
        private readonly WebServiceHost _serviceHost;

        public Host(string url) : 
            this(new WebServiceHost(typeof(TService), new Uri(url))) { }

        public Host(WebServiceHost serviceHost)
        {
            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().HttpHelpPageEnabled = false;
            serviceHost.Open();
            _serviceHost = serviceHost;
        }

        public RestResponse Get(string url)
        {
            return Execute(Method.GET, url, null);
        }

        public RestResponse Post(string url, string body)
        {
            return Execute(Method.POST, url, body);
        }

        public RestResponse Put(string url, string body)
        {
            return Execute(Method.PUT, url, body);
        }

        public RestResponse Delete(string url)
        {
            return Execute(Method.DELETE, url, null);
        }

        private RestResponse Execute(Method method, string url, string body)
        {
            var client = new RestClient { BaseUrl = _serviceHost.BaseAddresses[0].AbsoluteUri };
            var request = new RestRequest { Method = method, Resource = url };
            if (!string.IsNullOrEmpty(body)) request.AddBody(body);
            return client.Execute(request);
        }

        public void Dispose()
        {
            _serviceHost.Close();
        }
    }
}