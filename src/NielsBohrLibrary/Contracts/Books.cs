using System.Collections.Generic;
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
