using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Data.Models;
using ShelbyBooks.Logic.Exceptions;
using ShelbyBooks.Logic.Hubs;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;
    private readonly IHubContext<BookCountHub> _bookCountHubContext;

    public AddToCartCommandHandler(ShelbyBooksDbContext db, IUserService userService, IHubContext<BookCountHub> bookCountHubContext)
    {
        _db = db;
        _userService = userService;
        _bookCountHubContext = bookCountHubContext;
    }

    public async Task Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        // Находим пользователя и книгу
        var user = await _db.Users.Where(u => u.Id == currenUserId).FirstOrDefaultAsync(cancellationToken);
        var book = await _db.Books.Where(b => b.Id == request.BookId).FirstOrDefaultAsync(cancellationToken);

        if (book == null || user == null)
        {
            throw new NotFoundException("Не удалось добавить кингу в корзину.");
        }

        if (book.Quantity < 1)
        {
            throw new NotFoundException("Не удалось добавить кингу в корзину: данных книг не осталось.");
        }

        // Ищем запись в корзине
        var cart = await _db.Carts.Where(c => c.UserId == currenUserId && c.BookId == request.BookId)
            .FirstOrDefaultAsync(cancellationToken);
        if (cart != null)
        {
            cart.Quantity += 1;
        }
        else
        {
            await _db.Carts.AddAsync(new Cart(currenUserId, book.Id, 1, user, book), cancellationToken);
        }

        // Уменьшаем кол-во доступных книг
        book.Quantity -= 1;

        await _db.SaveChangesAsync(cancellationToken);
        
        // снова достаём книгу из бд
        book = await _db.Books.Where(b => b.Id == request.BookId).FirstOrDefaultAsync(cancellationToken);
        
        if (book == null)
        {
            throw new NotFoundException("Не удалось добавить кингу в корзину.");
        }
        // отправляем уведомление пользователям
        await _bookCountHubContext.Clients.All.SendAsync("updateBookCountResponse",book.Id, 
            book.Quantity, cancellationToken: cancellationToken);
    }
}