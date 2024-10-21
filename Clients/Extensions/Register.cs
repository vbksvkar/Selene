using Clients.Concretes;
using Clients.Interfaces;
using Clients.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models.Configuration;


namespace Clients.Extensions
{
    public static class Register
    {
        public static void RegisterClients(this IServiceCollection services)
        {
            services.AddTransient<AuthProvider>();
            services.AddTransient<RateLimitingProvider>();

            services.AddHttpClient<IAuthClient, AuthClient>((provider, client) => 
            {
                var options = provider.GetRequiredService<IOptions<SeleneConfig>>();
                var authUrl = options.Value.AuthUrl;
                client.BaseAddress = new Uri(authUrl);
            });

            services.AddHttpClient<IRedditClient, RedditClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<SeleneConfig>>();
                var redditUrl = options.Value.RedditUrl;
                client.BaseAddress = new Uri(redditUrl);
            })
            .AddHttpMessageHandler(provider => 
            {
                return provider.GetRequiredService<RateLimitingProvider>();
            })
            .AddHttpMessageHandler(provider =>
            {
                return provider.GetRequiredService<AuthProvider>();
            });
        }
    }
}