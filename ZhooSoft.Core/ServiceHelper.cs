namespace ZhooSoft.Core
{
    public static class ServiceHelper
    {
        #region Properties

        public static IServiceProvider? Services { get; private set; }

        #endregion

        #region Methods

        public static T? GetService<T>()
        {
            if (Services != null)
            {
                return Services.GetService<T>();
            }

            return default;
        }

        public static void Initialize(IServiceProvider serviceProvider) =>
            Services = serviceProvider;

        #endregion
    }
}
