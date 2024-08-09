namespace Orders.Application.Commands;
public class CreateOrderCommand : IRequest<int>
{
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool IsBuyOrder { get; set; }
}