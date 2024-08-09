using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orders.API.Hubs;
using Orders.Application.Commands;
using Orders.Application.Queries;
using Orders.Domain.Entities;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrdersController(IMediator mediator, IHubContext<OrderHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> Get()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);

        // Notify clients via SignalR
        await _hubContext.Clients.All.SendAsync("OrderCreated", command);

        return CreatedAtAction(nameof(Get), new { id = orderId }, orderId);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(command);

        if (result == 0)
        {
            return NotFound();
        }

        // Notify clients via SignalR
        await _hubContext.Clients.All.SendAsync("OrderUpdated", command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var command = new DeleteOrderCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result == 0)
        {
            return NotFound();
        }

        // Notify clients via SignalR
        await _hubContext.Clients.All.SendAsync("OrderDeleted", id);

        return Ok(result);
    }
}
