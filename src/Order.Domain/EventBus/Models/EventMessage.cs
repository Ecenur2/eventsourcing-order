namespace Order.Domain.EventBus.Models;

public class EventMessage
{
    public string Key { get; set; }
    
    public Dictionary<string,string> Headers { get; set; }
    
    public string SeriliazedPayload { get; set; }
}