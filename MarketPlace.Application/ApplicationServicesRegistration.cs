using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}