using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NielsBohrLibrary.Contracts
{
    [CollectionDataContract]
    public class Books : List<Book> 
    { 
        public Books() {}
        public Books(IEnumerable<Book> source) : base(source) {}
    }
}
