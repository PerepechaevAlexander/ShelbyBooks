using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.Exceptions;

namespace ShelbyBooks.Logic.Commands.ReceiveOrder;

public class ReceiveOrderCommandHandler : IRequestHandler<ReceiveOrderCommand>
{
    private readonly ShelbyBooksDbContext _db;

    public ReceiveOrderCommandHandler(ShelbyBooksDbContext db)
    {
        _db = db;
    }
    
    public async Task Handle(ReceiveOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.Where(o => o.Id == request.OrderId).FirstOrDefaultAsync(cancellationToken);
        if (order == null)
        {
            throw new NotFoundException("Не удалось обработать заказ.");
        }
        order.Status = "Выполнен";
        await _db.SaveChangesAsync(cancellationToken);
    }
}