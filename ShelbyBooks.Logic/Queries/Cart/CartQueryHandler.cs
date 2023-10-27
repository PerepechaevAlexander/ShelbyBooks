using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.DTO;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Queries.Cart;

public class CartQueryHandler : IRequestHandler<CartQuery, IList<BookDto>>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;

    public CartQueryHandler(ShelbyBooksDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    public async Task<IList<BookDto>> Handle(CartQuery request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        
        var books = await _db.Carts.Where(c => c.UserId == currenUserId).Select(cart => new BookDto
        {
            Id = cart.Book.Id,
            Isbn = cart.Book.Isbn,
            Title = cart.Book.Title,
            Author = cart.Book.Author,
            Year = cart.Book.Year,
            Image = cart.Book.Image,
            Cost = cart.Book.Cost,
            // Т.к. запись в корзине одна на несколько одинаковых книг -> берём кол-во книг из записи
            Quantity = cart.Quantity
        }).ToListAsync(cancellationToken);
        
        return books;
    }
}