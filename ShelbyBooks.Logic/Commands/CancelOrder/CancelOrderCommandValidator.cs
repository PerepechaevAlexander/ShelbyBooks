using FluentValidation;

namespace ShelbyBooks.Logic.Commands.CancelOrder;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(cancelOrderCommand => cancelOrderCommand.OrderId).GreaterThan(0);
    }
}