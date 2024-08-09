using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Infrastructure.Data;

namespace Orders.Application.Queries.Handlers;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
{
    private readonly ApplicationDbContext _context;

    public GetAllOrdersQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders.AsNoTracking().ToListAsync(cancellationToken);
    }
}