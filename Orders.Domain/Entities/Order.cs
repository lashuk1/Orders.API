namespace Orders.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool IsBuyOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsMatched { get; set; }

}