using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Models.Requests;
using Order.Api.Models.Responses;
using Order.Application.Commands.OrderCancel;
using Order.Application.Commands.OrderCreate;
using Order.Application.Commands.PrepareOrder;
using Order.Application.Commands.ShipmentFinish;
using Order.Application.Commands.ShipmentStart;
using Order.Application.Queries.Metrics;
using Order.Application.Queries.Order;
using Order.Domain.Events;

namespace Order.Api.Controllers;

[ApiController]
[Route("api/orders")]

public class OrderController : ControllerBase
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var order = await _sender.Send(new OrderQuery {OrderId = id});
        if(order == default) return  NotFound();
        
        return Ok(order.Adapt<OrderResponse>());
    }
    
    [HttpGet("metrics")]
    public async Task<IActionResult> Get()
    {
        var metrics = await _sender.Send(new MetricsQuery());
        if(metrics == default) return  NotFound();
        
        return Ok(metrics.Adapt<MetricsResponse>());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateOrderRequest request)
    {
        await _sender.Send(request.Adapt<OrderCreateCommand>());

        return Ok();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderRequest request)
    {
        var command = request.Adapt<OrderCancelCommand>();
        command.OrderId = id;
        await _sender.Send(command);

        return Ok();
    }

    [HttpPost("{id}/prepare")]
    public async Task<IActionResult> Prepare(Guid id, [FromBody] PrepareOrderRequest request)
    {
        var command = request.Adapt<PrepareOrderCommand>();
        command.OrderId = id;
        await _sender.Send(command);

        return Ok();
    }

    [HttpPost("{id}/shipment")]
    public async Task<IActionResult> Shipment(Guid id, [FromBody] ShipmentStartRequest request)
    {
        var command = request.Adapt<ShipmentStartCommand>();
        command.OrderId = id;
        await _sender.Send(command);

        return Ok();
    }
    
    [HttpPut("{id}/shipment")]
    public async Task<IActionResult> Shipment(Guid id, [FromBody] ShipmentFinishRequest request)
    {
        var command = request.Adapt<ShipmentFinishCommand>();
        command.OrderId = id;
        await _sender.Send(command);

        return Ok();
    }
}