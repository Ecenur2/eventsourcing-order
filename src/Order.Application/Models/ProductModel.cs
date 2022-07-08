namespace Order.Application.Models;

public class ProductModel
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
}