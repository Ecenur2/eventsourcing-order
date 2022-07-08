using Order.Domain.Attribute;
using Order.Domain.Constants;

namespace Order.Domain.Events;

[EventType(EventTypes.PreparedToCargo)]
public class PreparedToCargo : BaseEventPayload
{
    public decimal Weight { get; set; }
    
    public decimal Volume { get; set; }
}