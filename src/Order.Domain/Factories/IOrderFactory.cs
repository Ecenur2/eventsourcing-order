using Order.Domain.Aggregates;

namespace Order.Domain.Factories;

public interface IOrderFactory
{
    Task<OrderAggregateRoot> Get(Guid id);
}