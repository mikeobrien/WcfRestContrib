using System;

namespace WcfRestContrib.DependencyInjection
{
    public static class IDependencyResolverExtensions
    {
        public static T GetInfrastructureService<T>(this IDependencyResolver dependencyResolver, Type type)
        {
            return (T)dependencyResolver.GetInfrastructureService(type);
        }

        public static T GetInfrastructureService<T>(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.GetInfrastructureService<T>(typeof(T));
        }

        public static T GetOperationService<T>(this IDependencyResolver dependencyResolver, object container, Type type)
        {
            return (T)dependencyResolver.GetOperationService(container, type);
        }

        public static T GetOperationService<T>(this IDependencyResolver dependencyResolver, object container)
        {
            return dependencyResolver.GetOperationService<T>(container, typeof(T));
        }
    }
}
