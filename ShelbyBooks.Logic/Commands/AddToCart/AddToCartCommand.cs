using MediatR;

namespace ShelbyBooks.Logic.Commands.AddToCart;

public class AddToCartCommand : IRequest
{
    
    public readonly int BookId;

    public AddToCartCommand(int bookId)
    {
        BookId = bookId;
    }
}