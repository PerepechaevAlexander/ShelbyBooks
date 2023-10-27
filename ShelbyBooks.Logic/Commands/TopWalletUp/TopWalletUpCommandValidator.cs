using FluentValidation;

namespace ShelbyBooks.Logic.Commands.TopWalletUp;

public class TopWalletUpCommandValidator : AbstractValidator<TopWalletUpCommand>
{
    public TopWalletUpCommandValidator()
    {
        RuleFor(topWalletUpCommand => topWalletUpCommand.Amount).GreaterThan(0);
    }
}