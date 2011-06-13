using System;

namespace WcfRestContrib
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T target) where T : class
        {
            if (target == null) throw new NullReferenceException(
                string.Format("Object reference of type '{0}' not set to an instance of an object.", typeof(T).FullName));
            return target;
        }
    }
}