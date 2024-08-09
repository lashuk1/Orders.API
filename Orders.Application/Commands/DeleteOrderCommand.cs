namespace Orders.Application.Commands;
public class DeleteOrderCommand : IRequest<int>
{
    public int Id { get; set; }
}