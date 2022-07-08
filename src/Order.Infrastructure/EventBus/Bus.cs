using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Order.Domain.Constants;
using Order.Domain.EventBus;
using Order.Domain.EventBus.Models;
using Order.Domain.EventBus.Settings;
using Order.Domain.Events;
using Order.Domain.Extensions;
using Order.Domain.Repositories;
using Order.Domain.Repositories.Entities;
using Order.Infrastructure.Extensions;
using Order.Infrastructure.Helpers;

namespace Order.Infrastructure.EventBus;

public class Bus : IEventBus
{
    private readonly IMongoRepository _mongoRepository;
    private readonly KafkaHelper _kafkaHelper;
    private readonly EventBusSetting _eventBusSetting;
    public Bus(IMongoRepository mongoRepository, 
        KafkaHelper kafkaHelper, IOptions<EventBusSetting> eventBusSetting)
    {
        _mongoRepository = mongoRepository;
        _kafkaHelper = kafkaHelper;
        _eventBusSetting = eventBusSetting.Value;
    }
    public async Task Publish(string partitionKey,object payload)
    {
        var eventType = ((BaseEventPayload) payload).GetEventName();
        var orderTopicPartition = await UpsertPartition(EventConstants.OrderTopicName,partitionKey);
        var eventsTopicPartition = await UpsertPartition(EventConstants.EventTypeTopicName,eventType);
        var headers = new Dictionary<string, string> {{EventConstants.EventTypeHeaderName, eventType}};

        await _kafkaHelper.SendMessageToPartition(EventConstants.OrderTopicName, partitionKey, orderTopicPartition.PartitionId, headers, payload);
        await _kafkaHelper.SendMessageToPartition(EventConstants.EventTypeTopicName, eventType, eventsTopicPartition.PartitionId, headers, payload);

        await AddEventCount(new List<string>
        {
            orderTopicPartition.Key,
            eventsTopicPartition.Key
        });
    }

    public async Task<List<EventMessage>> ConsumeSlice(string topicName, string key, int startIndex)
    {
        var partition = await UpsertPartition(topicName,key);

        var messages = _kafkaHelper.GetPartitionMessages(topicName, partition.PartitionId, startIndex, partition.EventCount);

        return messages;
    }

    public async Task Consume(string topicName, IEventHandler handler)
    {
        var consumer = _kafkaHelper.BuildPersistentConsumer(handler.GetType().Name, topicName);

        while (true)
        {
            var result = consumer.Consume(TimeSpan.FromSeconds(_eventBusSetting.PersistentConsumerTimeout));
            if (result == default) continue;

            await handler.Handle(new EventMessage
            {
                Key = result.Message.Key,
                Headers = result.Message.Headers.ResolveHeaders(),
                SeriliazedPayload = result.Message.Value
            });
        }
    }

    private async Task<EventBusPartition> UpsertPartition(string topicName,string key)
    {
        var eventPartition = await _mongoRepository.GetAsync<EventBusPartition>(x => x.TopicName == topicName && x.Key == key);

        if (eventPartition != default)
        {
            return eventPartition;
        }

        var lastPartitionNumberOnTopic = await _mongoRepository.GetLastValueAsync<EventBusPartition>(x => x.TopicName == topicName);

        var partitionNumber = (lastPartitionNumberOnTopic?.PartitionId).GetValueOrDefault() + 1;
        var eventCount = default(int);
        
        var createPartitionTask = _kafkaHelper.CreateTopicPartition(topicName, partitionNumber);

        var addedEventPartition = new EventBusPartition
        {
            Key = key,
            TopicName = topicName,
            PartitionId = partitionNumber,
            EventCount = eventCount
        };
        var registerPartitionTask = _mongoRepository.InsertOneAsync(addedEventPartition);
        
        await Task.WhenAll(createPartitionTask, registerPartitionTask);

        return addedEventPartition;
    }
    
    private async Task AddEventCount(List<string> keys)
    {
        // todo performance
        var eventPartitions = await _mongoRepository.SearchAsync<EventBusPartition>(x => keys.Contains(x.Key),keys.Count);

        foreach (var eventBusPartition in eventPartitions)
        {
            eventBusPartition.EventCount++;
            await _mongoRepository.ReplaceOneAsync<EventBusPartition>(x => x.Id == eventBusPartition.Id, eventBusPartition);
        }
    }

    
}