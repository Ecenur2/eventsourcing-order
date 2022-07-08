using Mapster;
using MediatR;
using Order.Domain.Events;
using Order.Domain.Factories;

namespace Order.Application.Commands.PrepareOrder;

public class PrepareOrderCommandHandler : IRequestHandler<PrepareOrderCommand>
{
    private readonly IOrderFactory _orderFactory;

    public PrepareOrderCommandHandler(IOrderFactory orderFactory)
    {
        _orderFactory = orderFactory;
    }

    public async Task<Unit> Handle(PrepareOrderCommand request, CancellationToken cancellationToken)
    {
        var aggregateRoot = await _orderFactory.Get(request.OrderId);
        var orderPreparedToCargoEvent = request.Adapt<PreparedToCargo>();
        await aggregateRoot.Prepare(orderPreparedToCargoEvent);
        
        return Unit.Value;
    }
}