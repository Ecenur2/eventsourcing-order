using Order.Application;
using Order.EventConsumer;
using Order.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<OrderReadModelWriter>();
        services.AddHostedService<OrderMetricReadModelWriter>();
        services.AddInfrastructureServices(hostContext.Configuration);
        services.AddApplicationServices(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();
