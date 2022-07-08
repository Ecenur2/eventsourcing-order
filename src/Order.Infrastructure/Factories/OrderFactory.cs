using Order.Domain.Aggregates;
using Order.Domain.EventBus;
using Order.Domain.Factories;

namespace Order.Infrastructure.Factories;

public class OrderFactory : IOrderFactory
{
    private readonly IEventBus _eventBus;

    public OrderFactory(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task<OrderAggregateRoot> Get(Guid id)
    {
        var aggreageteRoot = new OrderAggregateRoot(id,_eventBus);
        await aggreageteRoot.Initialize();
        
        return aggreageteRoot;
    }
}