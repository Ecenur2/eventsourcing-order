using Order.Domain.Attribute;
using Order.Domain.Events;

namespace Order.Domain.Extensions;

public static class EventExtensions
{
    public static string GetEventName(this BaseEventPayload baseEvent)
    {
        return ((EventTypeAttribute) EventTypeAttribute.GetCustomAttribute(baseEvent.GetType(),
            typeof(EventTypeAttribute))).EventName;
    }
}