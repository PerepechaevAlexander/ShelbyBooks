using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelbyBooks.Logic.Commands.TopWalletUp;
using ShelbyBooks.Logic.Queries.Wallet;

namespace ShelbyBooks.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<decimal> Wallet()
    {
        var wallet = await _mediator.Send(new WalletQuery());
        return wallet;
    }
    
    [HttpPost("TopWalletUp")]
    public async Task<IActionResult> TopWalletUp([FromBody] decimal amount)
    {
        await _mediator.Send(new TopWalletUpCommand(amount));
        return Ok();
    }
}