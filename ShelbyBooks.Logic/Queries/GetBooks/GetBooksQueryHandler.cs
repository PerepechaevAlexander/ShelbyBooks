using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.DTO;

namespace ShelbyBooks.Logic.Queries.GetBooks;

public class GetBooksQueryHandler: IRequestHandler<GetBooksQuery, IList<BookDto>>
{
    private readonly ShelbyBooksDbContext _db;
    
    public GetBooksQueryHandler(ShelbyBooksDbContext db)
    {
        _db = db;
    }
    
    public async Task<IList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _db.Books.Select(book => new BookDto
        {
            Id = book.Id,
            Isbn = book.Isbn,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Image = book.Image,
            Cost = book.Cost,
            Quantity = book.Quantity
        }).ToListAsync(cancellationToken);
        return books;
    }
}