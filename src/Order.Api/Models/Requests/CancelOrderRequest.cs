using Order.Domain.Enums;

namespace Order.Api.Models.Requests;

public class CancelOrderRequest
{
    public OrderCancelReason CancelReason { get; set; }
}