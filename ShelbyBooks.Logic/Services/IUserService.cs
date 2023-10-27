namespace ShelbyBooks.Logic.Services;

public interface IUserService
{
    Task<int> GetCurrentUserIdAsync();
}