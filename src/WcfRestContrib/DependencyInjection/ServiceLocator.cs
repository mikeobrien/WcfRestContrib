namespace WcfRestContrib.DependencyInjection
{
    public static class ServiceLocator
    {
        static ServiceLocator()
        {
            Current = new DefaultObjectFactory();
        }

        public static bool IsDefault() { return Current is DefaultObjectFactory; }
        public static IObjectFactory Current { get; set; }
    }
}
