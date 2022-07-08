namespace Order.Domain.Attribute;

public class EventTypeAttribute : System.Attribute
{
    public string EventName { get; set; }

    public EventTypeAttribute(string eventName)
    {
        this.EventName = eventName;
    }
}