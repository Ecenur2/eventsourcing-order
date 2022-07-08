using MediatR;

namespace Order.Application.Queries.Order;

public class OrderQuery : IRequest<OrderQueryResult>
{
    public string OrderId { get; set; }
}