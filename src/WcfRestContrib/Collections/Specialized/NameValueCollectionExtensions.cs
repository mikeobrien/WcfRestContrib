using System.Collections.Generic;
using System.Collections.Specialized;

namespace WcfRestContrib.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        public static IEnumerable<KeyValuePair<string,string>> ToEnumerable(this NameValueCollection collection)
        {
            var list = new List<KeyValuePair<string, string>>();
            for (var index = 0; index < collection.Count; index++)
                list.Add(new KeyValuePair<string, string>(collection.Keys[index], collection[index]));
            return list;
        }
    }
}
