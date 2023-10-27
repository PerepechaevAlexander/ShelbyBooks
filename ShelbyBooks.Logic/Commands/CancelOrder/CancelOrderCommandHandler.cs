using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.Exceptions;
using ShelbyBooks.Logic.Hubs;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;
    private readonly IHubContext<BookCountHub> _bookCountHubContext;

    public CancelOrderCommandHandler(ShelbyBooksDbContext db, IUserService userService, IHubContext<BookCountHub> bookCountHubContext)
    {
        _db = db;
        _userService = userService;
        _bookCountHubContext = bookCountHubContext;
    }
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        
        var user = await _db.Users.Where(u => u.Id == currenUserId).FirstOrDefaultAsync(cancellationToken);
        var order = await _db.Orders.Where(o => o.Id == request.OrderId).FirstOrDefaultAsync(cancellationToken);
        var orderBooks = await _db.OrderBooks.Where(ob => ob.OrderId == request.OrderId).ToListAsync(cancellationToken);
        if (order == null || user == null || orderBooks == null)
        {
            throw new NotFoundException("Не удалось обработать заказ.");
        }
        // Меняем статус
        order.Status = "Отменён";
        user.Wallet += order.Cost;
        // Возвращаем книги на склад
        foreach (var ob in orderBooks)
        {
            var book = await _db.Books.Where(b => b.Id == ob.BookId).FirstOrDefaultAsync(cancellationToken);
            book!.Quantity += ob.Quantity;
        }
        
        await _db.SaveChangesAsync(cancellationToken);
        
        // снова достаём книгу из бд
        foreach (var ob in orderBooks)
        {
            var book = await _db.Books.Where(b => b.Id == ob.BookId).FirstOrDefaultAsync(cancellationToken);
            // отправляем уведомление пользователям
            await _bookCountHubContext.Clients.All.SendAsync("updateBookCountResponse",book!.Id, 
                book.Quantity, cancellationToken: cancellationToken);
        }
    }
}