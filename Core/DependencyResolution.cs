using Core.Entities;
using Core.Services;
using Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DependencyResolution
    {
        public static void RegisterCoreDependencies(this IServiceCollection services)
        {
            services.AddTransient<IGenericService<Bill>, GenericService<Bill>>();
            services.AddTransient<IGenericService<Company>, GenericService<Company>>();
            services.AddTransient<IGenericService<Order>, GenericService<Order>>();
            services.AddTransient<IGenericService<Product>, GenericService<Product>>();
            services.AddTransient<IGenericService<Stock>, GenericService<Stock>>();
            services.AddTransient<IGenericService<WorkPoint>, GenericService<WorkPoint>>();
            services.AddTransient<IGenericService<OrderProduct>, GenericService<OrderProduct>>();
            services.AddTransient<IOrderService, OrderService>();
        }
    }
}
