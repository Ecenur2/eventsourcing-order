using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Order.Domain.EventBus;
using Order.Domain.EventBus.Settings;
using Order.Domain.Factories;
using Order.Domain.Repositories;
using Order.Infrastructure.EventBus;
using Order.Infrastructure.Factories;
using Order.Infrastructure.Helpers;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure;

public static class Startup
{
    public static void AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddEventBus(configuration);
        services.Configure<EventBusSetting>(configuration.GetSection(nameof(EventBusSetting)));
        services.AddScoped<IOrderFactory, OrderFactory>();
    }
    
    public static void AddEventBus(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddEventBusRepository(configuration);
        services.AddSingleton<KafkaHelper>();
        services.AddScoped<IEventBus, Bus>();
    }

    public static IServiceCollection AddEventBusRepository(this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoUrl = configuration.GetValue<string>($"{nameof(EventBusSetting)}:{nameof(EventBusSetting.MongoConnectionUrl)}");
        var mongoUrlBuilder = new MongoUrlBuilder(mongoUrl);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrlBuilder.ToMongoUrl());

        services.AddSingleton<IMongoDatabase>(x =>
        {
            var mongoClient = new MongoClient(mongoClientSettings);
            var database = mongoClient.GetDatabase(mongoUrlBuilder.DatabaseName);
            
            return database;
        });

        services.AddScoped<IMongoRepository>(provider =>
        {
            var mongoDatabase = provider.GetServices<IMongoDatabase>()
                .Single(x => x.DatabaseNamespace.DatabaseName == mongoUrlBuilder.DatabaseName);

            return new MongoRepository(mongoDatabase);
        });

        return services;
    }
}