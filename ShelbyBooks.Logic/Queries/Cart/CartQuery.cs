using MediatR;
using ShelbyBooks.Logic.DTO;

namespace ShelbyBooks.Logic.Queries.Cart;

public class CartQuery : IRequest<IList<BookDto>>
{
    
}