using MediatR;

namespace ShelbyBooks.Logic.Commands.TopWalletUp;

public class TopWalletUpCommand : IRequest
{
    public readonly decimal Amount;

    public TopWalletUpCommand(decimal amount)
    {
        Amount = amount;
    }
}