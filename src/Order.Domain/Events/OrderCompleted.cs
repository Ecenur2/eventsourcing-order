using Order.Domain.Attribute;
using Order.Domain.Constants;

namespace Order.Domain.Events;

[EventType(EventTypes.OrderCompleted)]
public class OrderCompleted : BaseEventPayload
{
    
}