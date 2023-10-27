using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Queries.Wallet;

public class WalletQueryHandler : IRequestHandler<WalletQuery, decimal>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;

    public WalletQueryHandler(ShelbyBooksDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task<decimal> Handle(WalletQuery request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        
        var wallet = await _db.Users.Where(u => u.Id == currenUserId).Select(u => u.Wallet)
            .FirstOrDefaultAsync(cancellationToken) ;
        return wallet;
    }
}