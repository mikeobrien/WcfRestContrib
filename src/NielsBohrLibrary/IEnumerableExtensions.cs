using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NielsBohrLibrary
{
    public static class IEnumerableExtensions
    {
        public static bool Exists<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            foreach (T item in items)
                if (predicate(item)) return true;
            return false;
        }
    }
}
