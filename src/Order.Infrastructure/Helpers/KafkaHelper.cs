using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Order.Domain.EventBus.Models;
using Order.Domain.EventBus.Settings;
using Order.Infrastructure.Extensions;

namespace Order.Infrastructure.Helpers;

public class KafkaHelper
{
    private readonly EventBusSetting _eventBusSetting;
    private readonly ProducerConfig _producerConfig;
    private readonly IAdminClient _adminClient;
    
    public KafkaHelper(IOptions<EventBusSetting> eventBusSetting)
    {
        _eventBusSetting = eventBusSetting.Value;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = _eventBusSetting.BrokerList,
            SecurityProtocol = SecurityProtocol.Plaintext,
            Partitioner = Partitioner.Consistent
        };
        _adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = _eventBusSetting.BrokerList}).Build();
    }

    public async Task CreateTopic(string topicName, short replicationFactor, int numPartitions)
    {
        try
        {
            await _adminClient.CreateTopicsAsync(new TopicSpecification[]
            {
                new TopicSpecification
                {
                    Name = topicName,
                    ReplicationFactor = replicationFactor,
                    NumPartitions = numPartitions
                }
            });
                
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        }
    }
    
    public async Task CreateTopicPartition(string topicName,int partitionNumber)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = _eventBusSetting.BrokerList}).Build();
        var topicinfo = adminClient.GetMetadata(topicName,TimeSpan.FromSeconds(_eventBusSetting.ConnectionTimeout));
        var partitions = topicinfo?.Topics?.FirstOrDefault()?.Partitions;
        var partitionIndex = partitionNumber + 1;
        if (partitions?.Count >= partitionIndex) return;
        
        await adminClient.CreatePartitionsAsync(new PartitionsSpecification[]
        {
            new PartitionsSpecification
            {
                Topic = topicName,
                IncreaseTo = partitionIndex
            }
        });
    }
    
    public async Task SendMessageToPartition(string topicName,string key,int partitionNumber,Dictionary<string,string> headers,object message)
    {
        var messageHeaders = new Headers();
        foreach (var (headerKey, headerValue) in headers) messageHeaders.Add(headerKey,Encoding.ASCII.GetBytes(headerValue));
        using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();

        await producer.ProduceAsync(new TopicPartition(
            topicName, new Partition(partitionNumber)), new Message<string, string> {
            Key = key,
            Value = JsonSerializer.Serialize(message),
            Headers = messageHeaders
        });
    }

    public IConsumer<string, string> BuildPersistentConsumer(string consumerIdentity, string topicName)
    {
        var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            BootstrapServers = _eventBusSetting.BrokerList,
            SecurityProtocol = SecurityProtocol.Plaintext,
            GroupId = consumerIdentity,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        }).SetLogHandler((_, e) => Console.WriteLine($"Error: {e.Message}")).Build();

        consumer.Subscribe(topicName);
        return consumer;
    }

    public List<EventMessage> GetPartitionMessages(string topicName, int partitionNumber, int startIndex, int size)
    {
        var messages = new List<EventMessage>();
        if (size == default) return messages;
        var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            BootstrapServers = _eventBusSetting.BrokerList,
            SecurityProtocol = SecurityProtocol.Plaintext,
            GroupId = $"{topicName}-{partitionNumber}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        }).SetLogHandler((_, e) => Console.WriteLine($"Error: {e.Message}")).Build();
        var partitionOffSet = new TopicPartitionOffset(topicName,partitionNumber, startIndex);

        consumer.Assign(partitionOffSet);
        consumer.Seek(partitionOffSet);
        
        while (true)
        {
            if(messages.Count >= size) break;
            var result = consumer.Consume(TimeSpan.FromSeconds(_eventBusSetting.ConsumerTimeout));

            if (result != default)
                messages.Add(new EventMessage
                {
                    Key = result.Message.Key,
                    Headers = result.Message.Headers.ResolveHeaders(),
                    SeriliazedPayload = result.Message.Value
                });
        }
        
        return messages;
    }
    
    
}