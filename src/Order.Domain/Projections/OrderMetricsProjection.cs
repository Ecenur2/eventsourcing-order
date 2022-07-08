using Order.Domain.Constants;
using Order.Domain.Enums;
using Order.Domain.EventBus.Models;
using Order.Domain.Events;
using Order.Domain.Extensions;
using Order.Domain.ReadModels;

namespace Order.Domain.Projections;

public class OrderMetricsProjection
{
    private OrderMetricsReadModel _orderMetricsReadModel;
    
    public OrderMetricsReadModel Get => _orderMetricsReadModel;

    public OrderMetricsProjection(OrderMetricsReadModel orderMetricsReadModel,EventMessage eventPayload)
    {
        _orderMetricsReadModel = orderMetricsReadModel;
        ApplyEvent(eventPayload);
    }
    
    private void ApplyEvent(EventMessage eventPayload)
    {
        switch (eventPayload.Key)
        {
            case EventTypes.OrderCreated:
                _orderMetricsReadModel.TotalOrderCount += 1;
                break;

            case EventTypes.ShipmentStarted:
                _orderMetricsReadModel.OnDeliveryOrderCount += 1;
                break;

            case EventTypes.ShipmentFinished:
                _orderMetricsReadModel.OnDeliveryOrderCount -= 1;
                _orderMetricsReadModel.DeliveredOrderCount += 1;
                break;

            case EventTypes.OrderCancelled:
                _orderMetricsReadModel.CancelledOrderCount += 1;
                break;
        }
    }
}