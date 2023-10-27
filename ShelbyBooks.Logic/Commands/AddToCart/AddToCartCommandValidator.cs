using FluentValidation;

namespace ShelbyBooks.Logic.Commands.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(addToCartCommand => addToCartCommand.BookId).GreaterThan(0);
    }
}