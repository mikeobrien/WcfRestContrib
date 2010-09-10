using System;

namespace WcfRestContrib.Xml
{
    public static class TypeExtensions
    {
        public static string GetXsdType(this Type type)
        {
            if (type == typeof(Boolean)) return "Boolean";
            if (type == typeof(Byte)) return "UnsignedByte";
            if (type == typeof(DateTime)) return "Date";
            if (type == typeof(Decimal)) return "Decimal";
            if (type == typeof(Double)) return "Double";
            if (type == typeof(Int16)) return "Short";
            if (type == typeof(Int32)) return "Int";
            if (type == typeof(Int64)) return "Long";
            if (type == typeof(SByte)) return "Byte";
            if (type == typeof(Single)) return "Float";
            if (type == typeof(String)) return "String";
            if (type == typeof(TimeSpan)) return "Duration";
            if (type == typeof(Uri)) return "AnyURI";
            if (type == typeof(UInt32)) return "UnsignedInt";
            if (type == typeof(UInt64)) return "UnsignedLong";
            if (type == typeof(UInt16)) return "UnsignedShort";
            return string.Format("ComplexType:{0}", type.Name);
        }
    }
}
