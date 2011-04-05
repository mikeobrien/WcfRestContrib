using System;
using System.Collections.Generic;

namespace WcfRestContrib.DependencyInjection
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}
