using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.DTO;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Queries.Orders;

public class OrdersQueryHandler : IRequestHandler<OrdersQuery, IList<OrderDto>>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;

    public OrdersQueryHandler(ShelbyBooksDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task<IList<OrderDto>> Handle(OrdersQuery request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        
        var orders = await _db.Orders.Where(o => o.UserId == currenUserId).Select(o => new OrderDto
        {
            Id = o.Id,
            Status = o.Status,
            OrderBooks = o.OrderBooks.Select(ob=>new OrderBookDto
            {
                OrderId = ob.OrderId,
                Quantity = ob.Quantity,
                Title = ob.Book.Title
            }).ToList(),
            Cost = o.Cost,
        }).ToListAsync(cancellationToken);
        
        return orders;
    }
}