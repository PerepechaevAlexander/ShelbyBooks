namespace ShelbyBooks.Data.Models;

public class Order
{
    public Order()
    {
        
    }

    public Order(string status, decimal cost, int userId, User user)
    {
        Status = status;
        Cost = cost;
        UserId = userId;
        User = user;
    }
    
    public int Id { get; set; }
    public string? Status { get; set; }
    public decimal Cost { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<OrderBook> OrderBooks { get; set; }
}