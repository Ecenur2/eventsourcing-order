namespace Order.Api.Models.Requests;

public class CreateOrderRequest
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerSurName { get; set; }

    public List<CreateOrderProductRequest> Products { get; set; }

    public string CustomerAddress { get; set; }
}

public class CreateOrderProductRequest
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}