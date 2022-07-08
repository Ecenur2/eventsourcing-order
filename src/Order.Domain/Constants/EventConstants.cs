namespace Order.Domain.Constants;

public class EventConstants
{
    public const string OrderTopicName = "order";
    public const string EventTypeTopicName = "event-type";
    public const string EventTypeHeaderName = "event-type";
}

public static class EventTypes
{
    public const string OrderCreated = "OrderCreated";
    
    public const string PreparedToCargo = "PreparedToCargo";

    public const string ShipmentStarted = "ShipmentStarted";

    public const string ShipmentFinished = "ShipmentFinished";

    public const string OrderCompleted = "OrderCompleted";

    public const string OrderCancelled = "OrderCancelled";
}