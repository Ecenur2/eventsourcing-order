using MongoDB.Bson.Serialization.Attributes;

namespace Order.Domain.ReadModels;

public class OrderMetricsReadModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public int TotalOrderCount { get; set; }
    
    public int CancelledOrderCount { get; set; }
    
    public int OnDeliveryOrderCount { get; set; }
    
    public int DeliveredOrderCount { get; set; }
}