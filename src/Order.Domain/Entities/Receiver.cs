namespace Order.Domain.Entities;

public class Receiver
{
    public Customer Customer { get; set; }
    
    public string AddressDetail { get; set; }
}