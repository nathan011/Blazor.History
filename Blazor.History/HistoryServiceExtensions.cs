using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Blazor.History
{
    public static class HistoryServiceExtensions
    {
        public static IServiceCollection AddHistoryService(this IServiceCollection services)
        {
            services.AddScoped(serviceProvider => new HistoryService(serviceProvider.GetService<IJSRuntime>()));
            return services;
        }
    }
}
