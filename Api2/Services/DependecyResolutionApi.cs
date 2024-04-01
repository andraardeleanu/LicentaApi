using Api2.Services.Interfaces;
using FluentValidation.AspNetCore;
using FluentValidation;
using Api2.Validators;

namespace Api2.Services
{
    public static class DependecyResolutionApi
    {
        public static void RegisterApiDependencies(this IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CompanyValidator>();
            services.AddFluentValidationAutoValidation();
        }
    }
}
