using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;
using System.Net.Mime;
using WcfRestContrib.IO;

namespace NielsBohrLibrary.Domain
{
    public static class Catalog
    {
        // ────────────────────────── Private Fields ──────────────────────────

        private static List<CatalogItem> _catalog = new List<CatalogItem>();

        // ────────────────────────── Constructor ──────────────────────────

        static Catalog()
        {
            XDocument document = XDocument.Load(GetCatalogPath());

            var items = from item in document.Element("catalog").Elements("item")
                select new CatalogItem()
                {
                    Id = (int)item.Attribute("id"),
                    Title = (string)item.Attribute("title"),
                    Author = (string)item.Attribute("author"),
                    Description = (string)item.Attribute("description"),
                    Published = (int)item.Attribute("published"),
                    Isbn = (string)item.Attribute("isbn"),
                    Language = (string)item.Attribute("language"),
                    Summary = (string)item.Attribute("summary")
                };

            foreach (var item in items)
                _catalog.Add(item);
        }

        // ────────────────────────── Public Members ──────────────────────────

        public static IEnumerable<CatalogItem> GetCatalog()
        {
            return _catalog;
        }

        public static CatalogItem GetItem(string isbn)
        {
            return _catalog.FirstOrDefault(i => i.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
        }

        public static bool ItemExists(string isbn)
        {
            return _catalog.Exists(i => i.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
        }

        public static void AddItem(CatalogItem item)
        {
            lock (_catalog) { _catalog.Add(item); }
        }
    
        public static void UpdateItem(string isbn, CatalogItem item)
        {
            DeleteItem(isbn);
            AddItem(item);
        }

        public static void DeleteItem(string isbn)
        {
            CatalogItem item = _catalog.FirstOrDefault(
                i => i.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                lock (_catalog) { _catalog.Remove(item); }
        }

        public static bool SaveContentStream(Stream stream, string isbn)
        {
            string path = GetContentPath(isbn);
            bool exists = File.Exists(path);
            stream.Save(path);
            return exists;
        }

        public static bool TryGetContentStream(string isbn, out string filename, out string contentType, out Stream contentStream)
        {
            string path = GetContentPath(isbn);

            if (File.Exists(path))
            {
                filename = GenerateFilename(isbn);
                contentType = MediaTypeNames.Application.Pdf;
                contentStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                return true;
            }
            else
            {
                filename = null;
                contentType = null;
                contentStream = null;
                return false;
            }
        }

        // ────────────────────────── Private Members ──────────────────────────

        private static string GetCatalogPath()
        {
            return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"App_Data\Catalog.xml");
        }

        private static string GetContentPath(string isbn)
        {
            return Path.Combine(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        @"App_Data\Content"), 
                            GenerateFilename(isbn));
        }

        private static string GenerateFilename(string isbn)
        {
            return isbn + ".pdf";
        }
    }
}
