using Mapster;
using MapsterMapper;
using Order.Application.Commands.OrderCreate;
using Order.Application.Models;
using Order.Domain.Entities;
using Order.Domain.Events;

namespace Order.Application.Mappings;

public class CommandMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<OrderCreateCommand, OrderCreated>()
            .Map(dest => dest.Id , src => src.OrderId)
            .Map(dest => dest.AddressDetail , src => src.CustomerAddress)
            .Map(dest => dest.CreatedAt , src => DateTime.Now)
            ;
    }
}