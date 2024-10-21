using Microsoft.Extensions.DependencyInjection;

namespace Worker.Extensions
{
    public static class Register
    {
        public static void RegisterWorker(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();
        }
    }
}