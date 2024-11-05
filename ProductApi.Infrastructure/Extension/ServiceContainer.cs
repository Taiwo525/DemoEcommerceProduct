using Ecommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Persistence;
using ProductApi.Infrastructure.Repositories;
using System;
namespace ProductApi.Infrastructure.Extension
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // add database connectivity
            // ad authentication scheme
            SharedServiceContainer.AddSharedService<ProductDbContext>(services, config, config["MySerilog:FileNames"]);

            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // register middleware such as:
            // global exception to handles external error
            // listen to only api gateway to blocks all outsider calls.
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
