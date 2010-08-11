using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NielsBohrLibrary.Contracts
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Published { get; set; }

        [DataMember]
        public string Isbn { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string Summary { get; set; }
    }
}
