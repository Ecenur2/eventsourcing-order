using Newtonsoft.Json.Linq;
using Order.Domain.Constants;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.EventBus;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

namespace Order.Domain.Aggregates;

public class OrderAggregateRoot
{
    #region Properties
    
    public Guid Id { get; set; }

    public Receiver Receiver { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public List<OrderStatus> StatusHistory { get; set; } = new();
    
    public OrderCancelReason CancelReason { get; set; }
    
    public List<Product> Products { get; set; } = new();
    
    public Vehicle Vehicle { get; set; }

    public DateTime ShipmentDate { get; set; }

    public DateTime DeliveredDate { get; set; }

    public Package Package { get; set; }
    
    public int Version { get; set; } = -1;

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    #endregion

    private readonly IEventBus _eventBus;
    
    public OrderAggregateRoot(Guid id,IEventBus eventBus)
    {
        Id = id;
        _eventBus = eventBus;
    }

    public async Task Initialize()
    {
        var events = await _eventBus.ConsumeSlice(EventConstants.OrderTopicName, Id.ToString(), 0);
        foreach (var @event in events)
        {
            var eventType = @event.Headers[EventConstants.EventTypeTopicName];
            switch (eventType)
            {
                case EventTypes.OrderCreated:
                    Apply(@event.SeriliazedPayload.Deseriliaze<OrderCreated>());
                    Version++;
                    break;
                
                case EventTypes.PreparedToCargo:
                    Apply(@event.SeriliazedPayload.Deseriliaze<PreparedToCargo>());
                    Version++;
                    break;
                
                case EventTypes.ShipmentStarted:
                    Apply(@event.SeriliazedPayload.Deseriliaze<ShipmentStarted>());
                    Version++;
                    break;
                
                case EventTypes.ShipmentFinished:
                    Apply(@event.SeriliazedPayload.Deseriliaze<ShipmentFinished>());
                    Version++;
                    break;
                
                case EventTypes.OrderCompleted:
                    Apply(@event.SeriliazedPayload.Deseriliaze<OrderCompleted>());
                    Version++;
                    break;
                
                case EventTypes.OrderCancelled:
                    Apply(@event.SeriliazedPayload.Deseriliaze<OrderCancelled>());
                    Version++;
                    break;
            }
        }
    }
    
    #region Ctors
    
    
    
    #endregion

    #region ApplyMethods

    public void Apply(OrderCreated domainEvent)
    {
        Id = domainEvent.Id;
        Receiver = new Receiver
        {
            Customer = new Customer
            {
                Id = domainEvent.CustomerId,
                FullName = $"{domainEvent.CustomerName} {domainEvent.CustomerSurname}"
            },
            AddressDetail = domainEvent.AddressDetail
        };
        Status = OrderStatus.New;
        Products = Products;
        CreatedAt = domainEvent.CreatedAt;
    }

    public void Apply(PreparedToCargo domainEvent)
    {
        Package = new Package
        {
            Weight = domainEvent.Weight,
            Volume = domainEvent.Volume
        };
        ModifiedAt = domainEvent.CreatedAt;
        StatusHistory.Add(Status);
        Status = OrderStatus.Preparing;
        ModifiedAt = domainEvent.CreatedAt;
    }

    public void Apply(ShipmentStarted domainEvent)
    {
        Vehicle = domainEvent.VehicleType;
        ShipmentDate = domainEvent.CreatedAt;
        StatusHistory.Add(Status);
        Status = OrderStatus.AtCargo;
        ModifiedAt = domainEvent.CreatedAt;
    }
    
    public void Apply(ShipmentFinished domainEvent)
    {
        DeliveredDate = domainEvent.CreatedAt;
        StatusHistory.Add(Status);
        Status = OrderStatus.Delivered;
        ModifiedAt = domainEvent.CreatedAt;
    }

    public void Apply(OrderCompleted domainEvent)
    {
        StatusHistory.Add(Status);
        Status = OrderStatus.Completed;
        ModifiedAt = domainEvent.CreatedAt;
    }

    public void Apply(OrderCancelled domainEvent)
    {
        CancelReason = domainEvent.CancelReasonType;
        StatusHistory.Add(Status);
        Status = OrderStatus.Cancelled;
        ModifiedAt = domainEvent.CreatedAt;
    }
    
    #endregion

    #region Businesses

    public async Task CreateOrder(OrderCreated orderCreated)
    {
        // must be first event
        if (Version != -1) throw new OrderDomainException();
        
        await _eventBus.Publish(Id.ToString(),orderCreated);
    }
    
    public async Task Prepare(PreparedToCargo preparedToCargo)
    {
        if (Status != OrderStatus.New) throw new OrderDomainException();
        
        await _eventBus.Publish(Id.ToString(), preparedToCargo);
    }

    public async Task ShipmentStart(Vehicle vehicleType)
    {
        if (Status != OrderStatus.Preparing) throw new OrderDomainException();
        
        await _eventBus.Publish(Id.ToString(),new ShipmentStarted{VehicleType = vehicleType,CreatedAt = DateTime.Now});
    }
    
    public async Task Received(DateTime receivedDate)
    {
        if (Status != OrderStatus.AtCargo) throw new OrderDomainException();

        await _eventBus.Publish(Id.ToString(),new ShipmentFinished{CreatedAt = receivedDate});
        await _eventBus.Publish(Id.ToString(),new OrderCompleted {CreatedAt = receivedDate});
    }
    
    public async Task Cancel(OrderCancelReason reasonType)
    {
        await _eventBus.Publish(Id.ToString(),new OrderCancelled() {CancelReasonType = reasonType,CreatedAt = DateTime.Now});
    }
    
    #endregion
}