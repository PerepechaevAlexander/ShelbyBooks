using MediatR;

namespace ShelbyBooks.Logic.Commands.CancelOrder;

public class CancelOrderCommand : IRequest
{
    public readonly int OrderId;

    public CancelOrderCommand(int orderId)
    {
        OrderId = orderId;
    }
}