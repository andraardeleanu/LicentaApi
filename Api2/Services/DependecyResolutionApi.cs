using Api2.Services.Interfaces;

namespace Api2.Services
{
    public static class DependecyResolutionApi
    {
        public static void RegisterApiDependencies(this IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
        }
    }
}
