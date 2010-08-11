using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfRestContrib.Xml
{
    public static class TypeExtensions
    {
        public static string GetXsdType(this Type type)
        {
            if (type == typeof(System.Boolean)) return "Boolean";
            if (type == typeof(System.Byte)) return "UnsignedByte";
            if (type == typeof(System.DateTime)) return "Date";
            if (type == typeof(System.Decimal)) return "Decimal";
            if (type == typeof(System.Double)) return "Double";
            if (type == typeof(System.Int16)) return "Short";
            if (type == typeof(System.Int32)) return "Int";
            if (type == typeof(System.Int64)) return "Long";
            if (type == typeof(System.SByte)) return "Byte";
            if (type == typeof(System.Single)) return "Float";
            if (type == typeof(System.String)) return "String";
            if (type == typeof(System.TimeSpan)) return "Duration";
            if (type == typeof(System.Uri)) return "AnyURI";
            if (type == typeof(System.UInt32)) return "UnsignedInt";
            if (type == typeof(System.UInt64)) return "UnsignedLong";
            if (type == typeof(System.UInt16)) return "UnsignedShort";
            return string.Format("ComplexType:{0}", type.Name);
        }
    }
}
