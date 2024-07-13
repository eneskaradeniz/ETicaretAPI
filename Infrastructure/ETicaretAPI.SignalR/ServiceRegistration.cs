using ETicaretAPI.Application.Abstractions.Hubs;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection services)
        {
            services.AddTransient<IProductHubService, IProductHubService>();
            services.AddSignalR();
        }
    }
}
