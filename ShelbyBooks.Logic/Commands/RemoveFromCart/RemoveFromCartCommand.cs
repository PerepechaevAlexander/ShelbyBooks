using MediatR;

namespace ShelbyBooks.Logic.Commands.RemoveFromCart;

public class RemoveFromCartCommand : IRequest
{
    public readonly int BookId;

    public RemoveFromCartCommand(int bookId)
    {
        BookId = bookId;
    }
}