using MediatR;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Logic.Exceptions;
using ShelbyBooks.Logic.Services;

namespace ShelbyBooks.Logic.Commands.TopWalletUp;

public class TopWalletUpCommandHandler : IRequestHandler<TopWalletUpCommand>
{
    private readonly ShelbyBooksDbContext _db;
    private readonly IUserService _userService;

    public TopWalletUpCommandHandler(ShelbyBooksDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task Handle(TopWalletUpCommand request, CancellationToken cancellationToken)
    {
        var currenUserId = await _userService.GetCurrentUserIdAsync();
        var user = await _db.Users.Where(u => u.Id == currenUserId).FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException("Не удалось пополнить баланс: пользователь не найден.");
        }
        
        user.Wallet += request.Amount;
        
        await _db.SaveChangesAsync(cancellationToken);
    }
}