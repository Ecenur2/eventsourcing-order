namespace Order.Domain.EventBus.Models;

public class PublishEventModel
{
    public string TopicName { get; set; }
    
    public string Key { get; set; }
    
    public Dictionary<string,string> Headers { get; set; }
    
    public object Payload { get; set; }
}