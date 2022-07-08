namespace Order.Application.Queries.Metrics;

public class MetricsQueryResult
{
    public int TotalOrderCount { get; set; }
    
    public int CancelledOrderCount { get; set; }
    
    public int OnDeliveryOrderCount { get; set; }
    
    public int DeliveredOrderCount { get; set; }
}