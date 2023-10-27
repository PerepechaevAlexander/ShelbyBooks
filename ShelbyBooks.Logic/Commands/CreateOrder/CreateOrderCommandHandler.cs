using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Data.Models;
using ShelbyBooks.Logic.Exceptions;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;

    public CreateOrderCommandHandler(ShelbyBooksDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();

        var user = await _db.Users.Where(u => u.Id == currenUserId).FirstOrDefaultAsync(cancellationToken);
        var carts = await _db.Carts.Where(c => c.UserId == currenUserId).Include(cart => cart.Book)
            .ToListAsync(cancellationToken);
        var cost = carts.Select(c => c.Book.Cost * c.Quantity).Sum();
        
        
        if (user == null)
        {
            throw new NotFoundException("Не удалось создать заказ: пользователь не найден.");
        }
        if (carts.Count <= 0)
        {
            throw new NotFoundException("Не удалось создать заказ: корзина пуста.");
        }
        if (cost > user.Wallet)
        {
            throw new NotFoundException("Не удалось создать заказ: баланс недостаточен для совершения заказа.");
        }

        
        // Создаём запись заказа
        await _db.Orders.AddAsync(new Order("Новый", cost, currenUserId, user), cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var order = await _db.Orders.Where(o => o.UserId == currenUserId).OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync(cancellationToken);

        
        if (order == null)
        {
            throw new NotFoundException("Не удалось создать заказ.");
        }

        
        // Создаём записи OrderBooks, удаляем записи корзины и уменьшаем баланс
        user.Wallet -= cost;
        await _db.OrderBooks.AddRangeAsync(carts.Select(c => new OrderBook
        {
            OrderId = order.Id,
            BookId = c.Book.Id,
            Quantity = c.Quantity,
            Order = order,
            Book = c.Book
        }), cancellationToken);
        _db.Carts.RemoveRange(carts);

        await _db.SaveChangesAsync(cancellationToken);
    }
}