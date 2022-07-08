using Order.Domain.EventBus.Models;

namespace Order.Domain.EventBus;

public interface IEventBus
{
    Task Publish(string partitionKey, object payload);

    Task<List<EventMessage>> ConsumeSlice(string topicName, string key,int startIndex);
    
    Task Consume(string topicName, IEventHandler handler);
}