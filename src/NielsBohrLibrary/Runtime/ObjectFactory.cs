using System;
using WcfRestContrib.DependencyInjection;

namespace NielsBohrLibrary.Runtime
{
    public class ObjectFactory : IObjectFactory
    {
        public object Create(Type type)
        {
            // Insert your favorite DI tool here...
            return Activator.CreateInstance(type);
        }
    }
}