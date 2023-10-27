using MediatR;
using ShelbyBooks.Logic.DTO;

namespace ShelbyBooks.Logic.Queries.GetBooks;

public class GetBooksQuery : IRequest<IList<BookDto>>
{
    
}