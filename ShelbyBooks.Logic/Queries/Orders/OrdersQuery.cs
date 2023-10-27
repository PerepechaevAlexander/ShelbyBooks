using MediatR;
using ShelbyBooks.Logic.DTO;

namespace ShelbyBooks.Logic.Queries.Orders;

public class OrdersQuery : IRequest<IList<OrderDto>>
{
    
}