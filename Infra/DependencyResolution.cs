using Core.Interfaces;
using Infra.Data;
using Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra
{
    public static class DependencyResolution
    {
        public static void AddDatabaseContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)).AddOptions();
            services.RegisterRepositories();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
        }
    }
}
