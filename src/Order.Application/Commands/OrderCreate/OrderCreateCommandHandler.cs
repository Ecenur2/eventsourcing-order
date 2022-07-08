using Mapster;
using MediatR;
using Order.Domain.Events;
using Order.Domain.Factories;

namespace Order.Application.Commands.OrderCreate;

public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand>
{
    private readonly IOrderFactory _orderFactory;

    public OrderCreateCommandHandler(IOrderFactory orderFactory)
    {
        _orderFactory = orderFactory;
    }

    public async Task<Unit> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var aggregateRoot = await _orderFactory.Get(request.OrderId);
        var orderCreatedEvent = request.Adapt<OrderCreated>();
        await aggregateRoot.CreateOrder(orderCreatedEvent);
        
        return Unit.Value;
    }
}