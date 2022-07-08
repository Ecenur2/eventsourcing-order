using Mapster;
using Order.Domain.Constants;
using Order.Domain.Enums;
using Order.Domain.EventBus.Models;
using Order.Domain.Events;
using Order.Domain.Extensions;
using Order.Domain.ReadModels;

namespace Order.Domain.Projections;

public class OrderReadModelProjection
{
    private OrderReadModel _orderReadModel;

    public OrderReadModel Get => _orderReadModel;

    public OrderReadModelProjection(List<EventMessage> eventPayloads)
    {
        _orderReadModel = new OrderReadModel();
        foreach (var @event in eventPayloads)
        {
            ApplyEvent(@event);
        }
    }

    public OrderReadModelProjection(OrderReadModel orderReadModel,EventMessage eventPayload)
    {
        _orderReadModel = orderReadModel ?? new OrderReadModel();
        ApplyEvent(eventPayload);
    }
    
    private void ApplyEvent(EventMessage eventPayload)
    {
        var eventType = @eventPayload.Headers[EventConstants.EventTypeTopicName];
        switch (eventType)
        {
            case EventTypes.OrderCreated:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<OrderCreated>());
                break;

            case EventTypes.PreparedToCargo:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<PreparedToCargo>());
                break;

            case EventTypes.ShipmentStarted:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<ShipmentStarted>());
                break;

            case EventTypes.ShipmentFinished:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<ShipmentFinished>());
                break;

            case EventTypes.OrderCompleted:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<OrderCompleted>());
                break;

            case EventTypes.OrderCancelled:
                Apply(eventPayload.SeriliazedPayload.Deseriliaze<OrderCancelled>());
                break;
        }
    }

    #region ApplyMethods

    public void Apply(OrderCreated domainEvent)
    {
        _orderReadModel.OrderId = domainEvent.Id.ToString();
        _orderReadModel.CustomerId = domainEvent.CustomerId;
        _orderReadModel.CustomerName = $"{domainEvent.CustomerName} {domainEvent.CustomerSurname}";
        _orderReadModel.AddressDetail = domainEvent.AddressDetail;
        _orderReadModel.StatusName = OrderStatus.New.ToString();
        _orderReadModel.Products = domainEvent.Products.Adapt<List<ProductReadModel>>();
    }

    public void Apply(PreparedToCargo domainEvent)
    {
        _orderReadModel.PackageWeight = domainEvent.Weight;
        _orderReadModel.PackageVolume = domainEvent.Volume;
        _orderReadModel.StatusName = OrderStatus.Preparing.ToString();
    }

    public void Apply(ShipmentStarted domainEvent)
    {
        _orderReadModel.VehicleName = domainEvent.VehicleType.ToString();
        _orderReadModel.ShipmentDate = domainEvent.CreatedAt;
        _orderReadModel.StatusName = OrderStatus.AtCargo.ToString();
    }
    
    public void Apply(ShipmentFinished domainEvent)
    {
        _orderReadModel.DeliveredDate = domainEvent.CreatedAt;
        _orderReadModel.StatusName = OrderStatus.Delivered.ToString();
    }

    public void Apply(OrderCompleted domainEvent)
    {
        _orderReadModel.StatusName = OrderStatus.Completed.ToString();
    }

    public void Apply(OrderCancelled domainEvent)
    {
        _orderReadModel.CancelReasonName = domainEvent.CancelReasonType.ToString();
        _orderReadModel.StatusName = OrderStatus.Cancelled.ToString();

    }
    
    #endregion
}