using System;

namespace WcfRestContrib.DependencyInjection
{
    public interface IObjectFactory
    {
        object Create(Type type);
    }
}
