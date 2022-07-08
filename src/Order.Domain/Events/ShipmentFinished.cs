using Order.Domain.Attribute;
using Order.Domain.Constants;

namespace Order.Domain.Events;

[EventType(EventTypes.ShipmentFinished)]
public class ShipmentFinished : BaseEventPayload
{
    
}