using Microsoft.Extensions.DependencyInjection;

namespace App.Utils
{
    public static class AppServiceProvider
    {
        public static void Init(ServiceProvider provider)
        {
            _provider = provider;
        }

        private static ServiceProvider _provider;

        public static TService Get<TService>()
        {
            return _provider.GetService<TService>();
        }
    }
}