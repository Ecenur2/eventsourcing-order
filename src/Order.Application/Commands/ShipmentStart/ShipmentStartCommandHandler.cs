using MediatR;
using Order.Domain.Factories;

namespace Order.Application.Commands.ShipmentStart;

public class ShipmentStartCommandHandler : IRequestHandler<ShipmentStartCommand>
{
    private readonly IOrderFactory _orderFactory;

    public ShipmentStartCommandHandler(IOrderFactory orderFactory)
    {
        _orderFactory = orderFactory;
    }

    public async Task<Unit> Handle(ShipmentStartCommand request, CancellationToken cancellationToken)
    {
        var aggregateRoot = await _orderFactory.Get(request.OrderId);
        await aggregateRoot.ShipmentStart(request.VehicleType);
        
        return Unit.Value;
    }
}