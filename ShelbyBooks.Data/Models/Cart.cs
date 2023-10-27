namespace ShelbyBooks.Data.Models;

public class Cart
{
    public Cart()
    {
        
    }
    public Cart(int userId, int bookId, int quantity, User user, Book book)
    {
        UserId = userId;
        BookId = bookId;
        Quantity = quantity;
        User = user;
        Book = book;
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}