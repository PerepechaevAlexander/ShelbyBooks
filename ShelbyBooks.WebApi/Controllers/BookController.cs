using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShelbyBooks.Logic.DTO;
using ShelbyBooks.Logic.Queries.GetBooks;

namespace ShelbyBooks.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IList<BookDto>> GetBooks()
    {
        var books = await _mediator.Send(new GetBooksQuery());
        return books;
    }
}