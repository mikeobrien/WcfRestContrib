using System;

namespace WcfRestContrib.DependencyInjection
{
    public class DefaultObjectFactory : IObjectFactory
    {
        public static IObjectFactory Instance = new DefaultObjectFactory();

        public object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
