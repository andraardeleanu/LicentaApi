using Api2.Services.Interfaces;
using FluentValidation.AspNetCore;
using FluentValidation;
using Api2.Helpers.Interfaces;
using Api2.Helpers;

namespace Api2.Services
{
    public static class DependecyResolutionApi
    {
        public static void RegisterApiDependencies(this IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IBillGeneratorService, BillGeneratorService>();
            services.AddTransient<IDocumentHelper, DocumentHelper>();
            services.AddTransient<IDocumentConverterService, DocumentConverterService>();
            services.AddTransient<IOrderBillDocumentHelper, OrderBillDocumentHelper>();
        }

        //public static void ConfigureFluentValidation(this IServiceCollection services)
        //{
        //    services.AddValidatorsFromAssemblyContaining<CompanyValidator>();
        //    services.AddFluentValidationAutoValidation();
        //}
    }
}
