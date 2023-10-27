using MediatR;

namespace ShelbyBooks.Logic.Commands.ReceiveOrder;

public class ReceiveOrderCommand : IRequest
{
    public readonly int OrderId;

    public ReceiveOrderCommand(int orderId)
    {
        OrderId = orderId;
    }
}