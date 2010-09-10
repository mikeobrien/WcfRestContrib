using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WcfRestContrib.Web
{
    public class ServiceAnonymityModule : IHttpModule
    {
        // ────────────────────────── Private Fields ─────────────────────────────

        private static readonly IEnumerable<KeyValuePair<string, string>> ServiceMapping;

        // ────────────────────────── Constructors ───────────────────────────────

        static ServiceAnonymityModule()
        { ServiceMapping = GetServiceMapping(); }

        // ────────────────────────── IHttpModule Implementation ─────────────────

        public void Dispose() { }

        public void Init(HttpApplication app)
        {
            app.BeginRequest +=
                (s, e) => EnsureServiceMapping();
        }

        // ────────────────────────── Private Members ───────────────────────────

        private static void EnsureServiceMapping()
        {
            string path = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;

            Func<KeyValuePair<string, string>, bool> serviceMatch = map =>
                !path.StartsWith(map.Value) &&
                (
                    path.StartsWith(map.Key + '?', StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith(map.Key + '/', StringComparison.OrdinalIgnoreCase) ||
                    string.Compare(path, map.Key, true) == 0
                );

            var mapping = ServiceMapping.FirstOrDefault(serviceMatch);

            if (mapping.Key == null || mapping.Value == null) return;

            var pathInfo = path.Remove(0, mapping.Key.Length);
            if (string.IsNullOrEmpty(pathInfo)) pathInfo = "/";
            HttpContext.Current.RewritePath(
                mapping.Value, 
                pathInfo,
                HttpContext.Current.Request.QueryString.ToString());
        }

        private static IEnumerable<KeyValuePair<string, string>> GetServiceMapping()
        {
            var serviceMapping = new List<KeyValuePair<string, string>>();

            var webRoot = HttpContext.Current.Server.MapPath("~/");
            var serviceFiles = Directory.GetFiles(webRoot, "*.svc", SearchOption.AllDirectories);

            Func<string, bool, string> getRelative = (path, ext) =>
                "~/" +
                Path.Combine(Path.GetDirectoryName(path),
                    ext ?
                        Path.GetFileNameWithoutExtension(path) :
                        Path.GetFileName(path))
                    .Remove(0, webRoot.Length).Replace('\\', '/');

            var servicePaths = from servicePath in serviceFiles
                               orderby servicePath.Length descending
                               select new KeyValuePair<string, string>(
                                    getRelative(servicePath, true),
                                    getRelative(servicePath, false));

            serviceMapping.AddRange(servicePaths);
            return serviceMapping;
        }
    }
}
