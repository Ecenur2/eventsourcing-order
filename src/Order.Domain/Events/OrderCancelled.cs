using Order.Domain.Attribute;
using Order.Domain.Constants;
using Order.Domain.Enums;

namespace Order.Domain.Events;

[EventType(EventTypes.OrderCancelled)]
public class OrderCancelled : BaseEventPayload
{
    public OrderCancelReason CancelReasonType { get; set; }
}