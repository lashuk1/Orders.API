using MediatR;
using Orders.Infrastructure.Data;

namespace Orders.Application.Commands.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
{
    private readonly ApplicationDbContext _context;

    public DeleteOrderCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.Id);

        if (order == null)
        {
            return 0;
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}