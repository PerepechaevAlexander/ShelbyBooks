namespace ShelbyBooks.Logic.DTO;

public class OrderBookDto
{
    public int OrderId { get; set; } 
    public string Title { get; set; } = null!;
    public int Quantity { get; set; }
}