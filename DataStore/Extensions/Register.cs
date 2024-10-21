using DataStore.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace DataStore.Extensions
{
    public static class Register
    {
        public static void RegisterDataStore(this IServiceCollection services)
        {
            services.AddSingleton<IPostsStoreProvider, PostsStoreProvider>();
        }
    }
}