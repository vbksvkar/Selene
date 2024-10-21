using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Configuration;

namespace Models.Extensions
{
    public static class Register
    {
        public static void RegisterModels(this IHostApplicationBuilder builder, IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<SeleneConfig>(builder.Configuration);
        }
    }
}