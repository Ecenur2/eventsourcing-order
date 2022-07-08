using Order.Domain.Attribute;
using Order.Domain.Constants;
using Order.Domain.Enums;

namespace Order.Domain.Events;

[EventType(EventTypes.ShipmentStarted)]
public class ShipmentStarted : BaseEventPayload
{
    public Vehicle VehicleType { get; set; }
}