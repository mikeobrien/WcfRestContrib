using System;

namespace WcfRestContrib.Reflection
{
    public static class TypeExtensions
    {
        public static bool CastableAs<T>(this Type type) 
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static Type GetType<T>(this string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            Type type;
            try
            {
                type = Type.GetType(typeName);

                if (type == null)
                    throw new Exception(string.Format("Unable to find type {0}", typeName));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to find type {0}: {1}", typeName, e.Message), e);
            }

            if (type.CastableAs<T>())
                return type;
            throw new Exception(string.Format("{0} cannot be cast as {1}.", type.Name, typeof(T).Name));
        }
    }
}
