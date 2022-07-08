using MongoDB.Bson.Serialization.Attributes;

namespace Order.Domain.Repositories.Entities;

public class EventBusPartition
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    public string TopicName { get; set; }

    public string Key { get; set; }

    public int PartitionId { get; set; }

    public int EventCount { get; set; }
}