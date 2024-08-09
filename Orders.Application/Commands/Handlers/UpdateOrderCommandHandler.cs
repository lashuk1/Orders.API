using MediatR;
using Orders.Application.Services;
using Orders.Infrastructure.Data;

namespace Orders.Application.Commands.Handlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
{
    private readonly ApplicationDbContext _context;
    private readonly OrderMatcher _orderMatcher;

    public UpdateOrderCommandHandler(ApplicationDbContext context, OrderMatcher orderMatcher)
    {
        _context = context;
        _orderMatcher = orderMatcher;
    }

    public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.Id);

        if (order == null)
        {
            return 0;
        }

        order.UserId = request.UserId;
        order.Amount = request.Amount;
        order.Price = request.Price;
        order.IsBuyOrder = request.IsBuyOrder;

        await _context.SaveChangesAsync(cancellationToken);

        // Attempt to match the updated order
        await _orderMatcher.MatchOrderAsync(order);

        return order.Id;
    }
}
