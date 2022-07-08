using Mapster;
using MediatR;
using Order.Domain.Events;
using Order.Domain.Factories;

namespace Order.Application.Commands.ShipmentFinish;

public class ShipmentFinishCommandHandler : IRequestHandler<ShipmentFinishCommand>
{
    private readonly IOrderFactory _orderFactory;

    public ShipmentFinishCommandHandler(IOrderFactory orderFactory)
    {
        _orderFactory = orderFactory;
    }

    public async Task<Unit> Handle(ShipmentFinishCommand request, CancellationToken cancellationToken)
    {
        var aggregateRoot = await _orderFactory.Get(request.OrderId);
        await aggregateRoot.Received(request.ReceivedDate);
        
        return Unit.Value;
    }
}