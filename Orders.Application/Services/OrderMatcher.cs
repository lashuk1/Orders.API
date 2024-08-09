

namespace Orders.Application.Services;
public class OrderMatcher
{
    private readonly ApplicationDbContext _context;

    public OrderMatcher(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task MatchOrderAsync(Order newOrder)
    {
        var matchingOrders = _context.Orders
            .Where(o => o.IsBuyOrder != newOrder.IsBuyOrder && !o.IsMatched);

        if (newOrder.IsBuyOrder)
        {
            matchingOrders = matchingOrders.Where(o => o.Price <= newOrder.Price);
        }
        else
        {
            matchingOrders = matchingOrders.Where(o => o.Price >= newOrder.Price);
        }

        foreach (var order in matchingOrders)
        {
            if (newOrder.Amount <= 0) break;

            if (order.Amount == newOrder.Amount)
            {
                order.IsMatched = true;
                newOrder.IsMatched = true;
                break;
            }
            else if (order.Amount > newOrder.Amount)
            {
                order.Amount -= newOrder.Amount;
                newOrder.IsMatched = true;
                break;
            }
            else if (order.Amount < newOrder.Amount)
            {
                newOrder.Amount -= order.Amount;
                order.IsMatched = true;
            }
        }

        await _context.SaveChangesAsync();
    }
}

