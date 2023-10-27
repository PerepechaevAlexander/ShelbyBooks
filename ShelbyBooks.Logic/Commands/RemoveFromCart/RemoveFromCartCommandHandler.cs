using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.Exceptions;
using ShelbyBooks.Logic.Hubs;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;
    private readonly IHubContext<BookCountHub> _bookCountHubContext;

    public RemoveFromCartCommandHandler(ShelbyBooksDbContext db, IUserService userService, IHubContext<BookCountHub> bookCountHubContext)
    {
        _db = db;
        _userService = userService;
        _bookCountHubContext = bookCountHubContext;
    }

    public async Task Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        // Находим книгу и запись в корзине
        var book = await _db.Books.Where(b => b.Id == request.BookId).FirstOrDefaultAsync(cancellationToken);
        if (book == null)
        {
            throw new NotFoundException("Не удалось убрать кингу из корзины.");
        }
        
        var cart = await _db.Carts.Where(c => c.BookId == request.BookId && c.UserId == currenUserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (cart == null)
        {
            throw new NotFoundException("Не удалось убрать кингу из корзины.");
        }

        // Убираем книгу из корзину, добавляем кол-во дступных книг
        cart.Quantity -= 1;
        if (cart.Quantity == 0)
        {
            _db.Carts.Remove(cart);
        }

        book.Quantity += 1;

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