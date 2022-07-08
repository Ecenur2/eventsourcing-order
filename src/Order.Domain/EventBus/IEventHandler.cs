using Order.Domain.EventBus.Models;

namespace Order.Domain.EventBus;

public interface IEventHandler
{
    Task Handle(EventMessage message);
}