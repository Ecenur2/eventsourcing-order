using Order.Domain.Constants;
using Order.Domain.EventBus;
using Order.Domain.EventBus.Models;
using Order.Domain.Projections;
using Order.Domain.ReadModels;
using Order.Domain.Repositories;

namespace Order.EventConsumer;

public class OrderMetricReadModelWriter : BackgroundService,IEventHandler
{
    private readonly IEventBus _eventBus;
    private readonly IMongoRepository _mongoRepository;
    private readonly ILogger<OrderReadModelWriter> _logger;

    public OrderMetricReadModelWriter(IServiceScopeFactory serviceScopeFactory, ILogger<OrderReadModelWriter> logger)
    {
        _logger = logger;
        using var scope = serviceScopeFactory.CreateScope();
        _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        _mongoRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository>();
        _mongoRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("OrderMetricReadModelWriter started at: {time}", DateTimeOffset.Now);
                await _eventBus.Consume(EventConstants.EventTypeTopicName, this);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }

    public async Task Handle(EventMessage message)
    {
        var readModel = await _mongoRepository.GetAsync<OrderMetricsReadModel>(x => true);
        if (readModel == default)
        {
            readModel =  new OrderMetricsProjection(new OrderMetricsReadModel(), message).Get;
            await _mongoRepository.InsertOneAsync(readModel);
            
            return;
        }
        var newReadModel = new OrderMetricsProjection(readModel, message).Get;
        await _mongoRepository.ReplaceOneAsync(x => true,newReadModel);
    }
}