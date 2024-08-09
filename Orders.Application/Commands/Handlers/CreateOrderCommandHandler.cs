using MediatR;
using Orders.Application.Services;
using Orders.Domain.Entities;
using Orders.Infrastructure.Data;

namespace Orders.Application.Commands.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly ApplicationDbContext _context;
    private readonly OrderMatcher _orderMatcher;

    public CreateOrderCommandHandler(ApplicationDbContext context, OrderMatcher orderMatcher)
    {
        _context = context;
        _orderMatcher = orderMatcher;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = request.UserId,
            Amount = request.Amount,
            Price = request.Price,
            IsBuyOrder = request.IsBuyOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Attempt to match the order
        await _orderMatcher.MatchOrderAsync(order);

        return order.Id;
    }
}
