namespace ShelbyBooks.Logic.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public List<OrderBookDto> OrderBooks { get; set; } = null!;
    public decimal Cost { get; set; }
}