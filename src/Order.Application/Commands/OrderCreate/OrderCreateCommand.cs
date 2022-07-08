using MediatR;
using Order.Application.Models;

namespace Order.Application.Commands.OrderCreate;

public class OrderCreateCommand : IRequest
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerSurName { get; set; }

    public List<ProductModel> Products { get; set; }

    public string CustomerAddress { get; set; }
}