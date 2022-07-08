using Mapster;
using MediatR;
using Order.Domain.ReadModels;
using Order.Domain.Repositories;

namespace Order.Application.Queries.Order;

public class OrderQueryHandler : IRequestHandler<OrderQuery,OrderQueryResult>
{
    private readonly IMongoRepository _mongoRepository;

    public OrderQueryHandler(IMongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<OrderQueryResult> Handle(OrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _mongoRepository.GetAsync<OrderReadModel>(x => x.OrderId == request.OrderId);

        return order?.Adapt<OrderQueryResult>();
    }
}