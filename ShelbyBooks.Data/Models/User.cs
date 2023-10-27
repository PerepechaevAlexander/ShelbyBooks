namespace ShelbyBooks.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal Wallet { get; set; }
}