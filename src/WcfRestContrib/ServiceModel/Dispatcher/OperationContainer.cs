using System.ServiceModel;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    public static class OperationContainer
    {
        public static bool Exists()
        {
            return GetContainerExtension() != null;
        }

        public static object GetCurrent()
        {
            var extension = GetContainerExtension();
            if (extension == null)
            {
                extension = new ContainerExtension(DependencyResolver.Current.CreateOperationContainer());
                OperationContext.Current.Extensions.Add(extension);
            }
            return extension.Container;
        }

        private static ContainerExtension GetContainerExtension()
        {
            return OperationContext.Current.Extensions.Find<ContainerExtension>();
        }

        public class ContainerExtension : IExtension<OperationContext>
        {
            public ContainerExtension(object container) { Container = container; }
            public object Container { get; private set; }
            public void Attach(OperationContext owner) { }
            public void Detach(OperationContext owner) { }
        } 
    }
}