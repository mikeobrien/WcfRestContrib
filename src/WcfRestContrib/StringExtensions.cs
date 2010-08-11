using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfRestContrib
{
    public static class StringExtensions
    {
        public static string Ellipsis(this string value, int maxLength)
        {
            return Ellipsis(value, maxLength, string.Empty, string.Empty);
        }

        public static string Ellipsis(this string value, int maxLength, string nullValue, string emptyValue)
        {
            if (value == null) return nullValue;
            if (string.IsNullOrEmpty(value)) return emptyValue;
            if (value.Length <= maxLength) return value;

            return value.Substring(0, maxLength) + "...";
        }
    }
}
