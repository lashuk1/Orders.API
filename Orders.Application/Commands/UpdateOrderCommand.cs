namespace Orders.Application.Commands;
public class UpdateOrderCommand : IRequest<int>
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool IsBuyOrder { get; set; }
}