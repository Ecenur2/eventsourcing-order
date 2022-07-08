namespace Order.Api.Models.Responses;

public class MetricsResponse
{
    public int TotalOrderCount { get; set; }
    
    public int CancelledOrderCount { get; set; }
    
    public int OnDeliveryOrderCount { get; set; }
    
    public int DeliveredOrderCount { get; set; }
}