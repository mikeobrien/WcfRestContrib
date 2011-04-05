using System;

namespace WcfRestContrib.DependencyInjection
{
    public static class IDependencyResolverExtensions
    {
        public static T Create<T>(this IDependencyResolver dependencyResolver, Type type)
        {
            return (T)dependencyResolver.GetService(type);
        }

        public static T Create<T>(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.Create<T>(typeof(T));
        }
    }
}
