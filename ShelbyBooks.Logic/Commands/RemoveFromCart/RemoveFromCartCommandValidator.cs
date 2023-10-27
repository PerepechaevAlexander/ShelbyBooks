using FluentValidation;

namespace ShelbyBooks.Logic.Commands.RemoveFromCart;

public class RemoveFromCartCommandValidator : AbstractValidator<RemoveFromCartCommand>
{
    public RemoveFromCartCommandValidator()
    {
        RuleFor(removeFromCartCommand => removeFromCartCommand.BookId).GreaterThan(0);
    }
}