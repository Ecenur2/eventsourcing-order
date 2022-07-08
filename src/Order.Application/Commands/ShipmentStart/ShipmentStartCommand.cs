using MediatR;
using Order.Domain.Enums;

namespace Order.Application.Commands.ShipmentStart;

public class ShipmentStartCommand : IRequest
{
    public Guid OrderId { get; set; }

    public Vehicle VehicleType { get; set; }
}