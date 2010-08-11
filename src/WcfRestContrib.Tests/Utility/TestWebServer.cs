using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.WebHost;
using WcfRestContrib.Net.Http;

namespace WcfRestContrib.Tests.Utility
{
    /// <summary>
    /// <para>A Web Server useful for unit tests.  Uses the same code used by the
    /// built in WebServer (formerly known as Cassini) in VisualStudio.NET 2008.
    /// Specifically, this needs a reference to WebServer.WebHost.dll - the pre-build
    /// will copy this to ..\_lib.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Based on http://haacked.com/archive/2006/12/12/Using_WebServer.WebDev_For_Unit_Tests.aspx
    /// </para>
    /// </remarks>
    public sealed class TestWebServer : IDisposable
    {
        private Server _webServer;
        private readonly string _webRoot;
        private readonly int _webServerPort;
        private readonly string _webServerVDir;
        private string _webServerUrl; //built in Start

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWebServer"/> class
        /// using the specified port and virtual dir.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="virtualDir">The virtual dir.</param>
        /// <param name="webRoot">The physical path (can be expressed relative)</param>
        public TestWebServer(int port, string virtualDir, string webRoot)
        {
            _webServerPort = port;
            _webServerVDir = virtualDir;
            _webRoot = Path.GetFullPath(webRoot);
        }

        #region Dispose/finalize

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        /// <remarks>
        /// If we unseal this class, make sure this is protected virtual.
        /// </remarks>
        ///<filterpriority>2</filterpriority>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();
            }
        }

        ~TestWebServer()
        {
            Dispose(false);
        }

        // Cleans up the directories we created.
        private void ReleaseManagedResources()
        {
            if (_webServer != null)
            {
                _webServer.Stop();
                _webServer = null;
            }
        } 

        #endregion

        #region Public methods

        /// <summary>
        /// Starts the webserver and returns the URL.
        /// </summary>
        public Uri Start()
        {
            //Start the internal Web Server pointing to our test webroot
            _webServer = new Server(_webServerPort, _webServerVDir, _webRoot);
            _webServerUrl = String.Format("http://localhost:{0}{1}", _webServerPort, _webServerVDir);

            _webServer.Start();
            Debug.WriteLine(String.Format("Web Server started on port {0} with VDir {1} in physical directory {2}",
                                          _webServerPort, _webServerVDir, _webRoot));
   
            return new Uri(_webServerUrl);
        }

        /// <summary>
        /// Gets a string response from a REST Uri.
        /// </summary>
        /// <param name="rest">Details of things we want in the REST request.</param>
        /// <returns></returns>
        public string GetResponse(TestRestRequest rest)
        {
            HttpWebResponse response = Invoke(rest);
            using(var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Makes a  simple GET request to the web server and returns
        /// the result as an XDocument.
        /// </summary>
        public XDocument GetXDocument(string uri)
        {
            using(HttpWebResponse response = Invoke(new TestRestRequest(Verbs.Get, uri)))
            {
                using(var reader = XmlReader.Create(response.GetResponseStream()))
                {
                    return XDocument.Load(reader);
                }
            }
        }

        /// <summary>
        /// Invokes the given request.
        /// </summary>
        /// <param name="rest">Details of things we want in the REST request.</param>
        /// <returns>A vanilla HttpWebResponse</returns>
        public HttpWebResponse Invoke(TestRestRequest rest)
        {
            var request = (HttpWebRequest)WebRequest.Create(
                new Uri(new Uri(_webServerUrl), rest.Uri).ToString());
            request.AllowAutoRedirect = true;
            request.Method = rest.Verb;
            request.ContentType = rest.ContentType;
            request.Credentials = rest.Credentials;
            if(!string.IsNullOrEmpty(rest.Accept))
                request.Accept = rest.Accept;

            if (rest.Body != null)
            {
                request.ContentLength = Encoding.UTF8.GetByteCount(rest.Body);
                Stream bodyStream = request.GetRequestStream();
                bodyStream.Write(Encoding.UTF8.GetBytes(rest.Body), 0, (int)request.ContentLength);
            }

            if (rest.Headers != null)
                request.Headers.Add(rest.Headers);

            Console.WriteLine(request.AsDebugString());
            var response = (HttpWebResponse) request.GetResponse();
            Console.WriteLine("\t... {0}", response.StatusCode);
            return response;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            Dispose();
        } 

        #endregion

        public override string ToString()
        {
            return _webServerUrl;
        }
    }
}