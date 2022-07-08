using MediatR;
using Order.Domain.Enums;

namespace Order.Application.Commands.OrderCancel;

public class OrderCancelCommand : IRequest
{
    public Guid OrderId { get; set; }
    
    public OrderCancelReason CancelReason { get; set; }
}