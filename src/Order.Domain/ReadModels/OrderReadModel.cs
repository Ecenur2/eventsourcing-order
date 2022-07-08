using MongoDB.Bson.Serialization.Attributes;
using Order.Domain.Enums;

namespace Order.Domain.ReadModels;

public class OrderReadModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string OrderId { get; set; }
    
    public Guid? CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string AddressDetail { get; set; }

    public string StatusName { get; set; }

    public string CancelReasonName { get; set; }
    
    public List<ProductReadModel> Products { get; set; } = new();

    public string VehicleName { get; set; }

    public DateTime? ShipmentDate { get; set; }

    public DateTime? DeliveredDate { get; set; }
    
    public decimal? PackageWeight { get; set; }
    
    public decimal? PackageVolume { get; set; }
}

public class ProductReadModel
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
}