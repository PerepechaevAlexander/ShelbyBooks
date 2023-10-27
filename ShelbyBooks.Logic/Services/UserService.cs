using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data;
using ShelbyBooks.Data.Models;

namespace ShelbyBooks.Logic.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly ShelbyBooksDbContext _db;

    public UserService(IHttpContextAccessor accessor, ShelbyBooksDbContext db)
    {
        _accessor = accessor;
        _db = db;
    }
    public async Task<int> GetCurrentUserIdAsync()
    {
        var userClaims = _accessor.HttpContext?.User;

        var email = userClaims?.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            throw new Exception("Не удалось аутентифицировать пользователя: Email = null.");
        }
        // Ищем пользователя в БД по email
        var userId = await _db.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefaultAsync();
        if (userId != 0)
        {
            return userId;
        }
        // Если пользователя не найден, а email в httpContext есть -> создаём пользователя
        var name = userClaims?.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(name))
        {
            name = "unknown";
        }
        await _db.Users.AddAsync(new User{Email = email, Login = email, Name = name, Wallet = 0});
        await _db.SaveChangesAsync();
        userId = await _db.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefaultAsync();
        if (userId != 0)
        {
            return userId;
        }
        return 0;
    }
    
    
}