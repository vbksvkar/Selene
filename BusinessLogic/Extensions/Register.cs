using BusinessLogic.Concretes;
using BusinessLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Extensions
{
    public static class Register
    {
        public static void RegisterBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<IRedditService, RedditService>();
        }
    }
}