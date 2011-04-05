namespace WcfRestContrib.DependencyInjection
{
    public static class DependencyResolver
    {
        static DependencyResolver()
        {
            Current = new DefaultDependencyResolver();
        }

        public static bool IsDefault() { return Current is DefaultDependencyResolver; }
        public static IDependencyResolver Current { get; private set; }

        public static void SetResolver(IDependencyResolver resolver)
        {
            Current = resolver;
        }
    }
}
