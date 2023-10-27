namespace ShelbyBooks.Data.Models;

public class OrderBook
{
    public OrderBook()
    {
        
    }

    public OrderBook(int orderId, int bookId, int quantity, Order order, Book book)
    {
        OrderId = orderId;
        BookId = bookId;
        Quantity = quantity;
        Order = order;
        Book = book;
    }
    
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public Order Order { get; set; } = null!;
    public Book Book { get; set; } = null!;
}