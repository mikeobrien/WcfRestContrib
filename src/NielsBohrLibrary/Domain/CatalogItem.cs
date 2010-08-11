using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NielsBohrLibrary.Domain
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Published { get; set; }
        public string Isbn { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }
    }
}
