using Order.Domain.Enums;

namespace Order.Api.Models.Requests;

public class ShipmentStartRequest
{
    public Vehicle VehicleType { get; set; }
}