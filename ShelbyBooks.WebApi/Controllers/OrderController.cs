using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelbyBooks.Logic.Commands.CancelOrder;
using ShelbyBooks.Logic.Commands.CreateOrder;
using ShelbyBooks.Logic.Commands.ReceiveOrder;
using ShelbyBooks.Logic.DTO;
using ShelbyBooks.Logic.Queries.Orders;

namespace ShelbyBooks.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IList<OrderDto>> GetOrders()
    {
        var res = await _mediator.Send(new OrdersQuery());
        return res;
    }
    
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder()
    {
        await _mediator.Send(new CreateOrderCommand());
        return Ok();
    }
    
    [HttpPost("ReceiveOrder")]
    public async Task<IActionResult> ReceiveOrder([FromBody] int orderId)
    {
        await _mediator.Send(new ReceiveOrderCommand(orderId));
        return Ok();
    }
    
    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder([FromBody] int orderId)
    {
        await _mediator.Send(new CancelOrderCommand(orderId));
        return Ok();
    }
    
}