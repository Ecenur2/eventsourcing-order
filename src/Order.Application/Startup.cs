using System.Reflection;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Application;

public static class Startup
{
    public static void AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatR(typeof(Startup));

        services.AddMappings();
    }
    public static IServiceCollection AddMappings(this IServiceCollection serviceCollection)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(Application.Startup).Assembly);
        
        return serviceCollection;
    }
}