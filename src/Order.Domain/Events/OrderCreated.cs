using Order.Domain.Attribute;
using Order.Domain.Constants;
using Order.Domain.Entities;

namespace Order.Domain.Events;

[EventType(EventTypes.OrderCreated)]
public class OrderCreated : BaseEventPayload
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; }
    
    public string CustomerSurname { get; set; }
    
    public List<Product> Products { get; set; }
    
    public string AddressDetail { get; set; }
}