namespace ShelbyBooks.Data.Models;

public class Book
{
    public int Id { get; set; }
    public string? Isbn { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Year { get; set; }
    public byte[]? Image { get; set; }
    public decimal Cost { get; set; }
    public int Quantity { get; set; }
}