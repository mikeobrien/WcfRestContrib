using System;

namespace WcfRestContrib.DependencyInjection
{
    public static class IObjectFactoryExtensions
    {
        public static T Create<T>(this IObjectFactory objectFactory, Type type)
        {
            return (T)objectFactory.Create(type);
        }

        public static T Create<T>(this IObjectFactory objectFactory)
        {
            return objectFactory.Create<T>(typeof(T));
        }

    }
}
