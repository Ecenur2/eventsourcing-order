namespace Order.Api.Models.Responses;

public class OrderResponse
{
    public string OrderId { get; set; }
    
    public Guid? CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string AddressDetail { get; set; }

    public string StatusName { get; set; }

    public string CancelReasonName { get; set; }
    
    public List<OrderResponseProductItem> Products { get; set; } = new();

    public string VehicleName { get; set; }

    public DateTime? ShipmentDate { get; set; }

    public DateTime? DeliveredDate { get; set; }
    
    public decimal? PackageWeight { get; set; }
    
    public decimal? PackageVolume { get; set; }
}

public class OrderResponseProductItem
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
}