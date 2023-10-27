using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelbyBooks.Logic.Commands.AddToCart;
using ShelbyBooks.Logic.Commands.RemoveFromCart;
using ShelbyBooks.Logic.DTO;
using ShelbyBooks.Logic.Queries.Cart;

namespace ShelbyBooks.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IList<BookDto>> Cart()
    {
        var books = await _mediator.Send(new CartQuery());
        return books;
    }
    
    [HttpPost("AddToCart")]
    public async Task<IActionResult> AddToCart([FromBody] int bookId)
    {
        await _mediator.Send(new AddToCartCommand(bookId));
        return Ok();
    }
    
    [HttpPost("RemoveFromCart")]
    public async Task<IActionResult> RemoveFromCart([FromBody] int bookId)
    {
        await _mediator.Send(new RemoveFromCartCommand(bookId));
        return Ok();
    }
}