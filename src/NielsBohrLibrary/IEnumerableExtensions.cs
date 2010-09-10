using System;
using System.Collections.Generic;
using System.Linq;

namespace NielsBohrLibrary
{
    public static class IEnumerableExtensions
    {
        public static bool Exists<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            return items.Any(item => predicate(item));
        }
    }
}
