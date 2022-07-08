using MediatR;

namespace Order.Application.Commands.ShipmentFinish;

public class ShipmentFinishCommand : IRequest
{
    public Guid OrderId { get; set; }

    public DateTime ReceivedDate { get; set; }
}