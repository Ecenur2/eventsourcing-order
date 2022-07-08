using MediatR;

namespace Order.Application.Commands.PrepareOrder;

public class PrepareOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
    
    public decimal Weight { get; set; }
    
    public decimal Volume { get; set; }
}