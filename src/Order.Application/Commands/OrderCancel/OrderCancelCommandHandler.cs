using MediatR;
using Order.Domain.Factories;

namespace Order.Application.Commands.OrderCancel;

public class OrderCancelCommandHandler : IRequestHandler<OrderCancelCommand>
{
    private readonly IOrderFactory _orderFactory;

    public OrderCancelCommandHandler(IOrderFactory orderFactory)
    {
        _orderFactory = orderFactory;
    }
    
    public async Task<Unit> Handle(OrderCancelCommand request, CancellationToken cancellationToken)
    {
        var aggregateRoot = await _orderFactory.Get(request.OrderId);
        await aggregateRoot.Cancel(request.CancelReason);
        
        return Unit.Value;
    }
}