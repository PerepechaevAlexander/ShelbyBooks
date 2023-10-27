using FluentValidation;

namespace ShelbyBooks.Logic.Commands.ReceiveOrder;

public class ReceiveOrderCommandValidator : AbstractValidator<ReceiveOrderCommand>
{
    public ReceiveOrderCommandValidator()
    {
        RuleFor(receiveOrderCommand => receiveOrderCommand.OrderId).GreaterThan(0);
    }
}