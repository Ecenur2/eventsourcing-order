namespace Order.Api.Models.Requests;

public class PrepareOrderRequest
{
    public decimal Weight { get; set; }
    
    public decimal Volume { get; set; }
}