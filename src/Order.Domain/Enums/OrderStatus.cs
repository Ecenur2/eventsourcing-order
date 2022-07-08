namespace Order.Domain.Enums;

public enum OrderStatus
{
    None = 0,
    New,
    Preparing,
    AtCargo,
    Delivered,
    Completed,
    Cancelled
}