using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Dashboard;

public class GetLowStockItems
{
    public class Query : IRequest<List<InventoryItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Query, List<InventoryItemDto>>
    {
        private readonly AppDbContext _context;
        public Handler(AppDbContext context) => _context = context;

        public async Task<List<InventoryItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.InventoryItems
                .Where(i =>
                    i.UserId == request.UserId &&
                    i.IsActive &&
                    i.Quantity < i.MinStockLevel)
                .Select(i => new InventoryItemDto
                {
                    Id = i.Id,
                    Symbol = i.Symbol,
                    Category = i.Category,
                    Package = i.Package,
                    Value = i.Value,
                    Quantity = i.Quantity,
                    MinStockLevel = i.MinStockLevel,
                    Location = i.Location,
                    ReservedForProjects = i.ReservedForProjects,
                    DatasheetLink = i.DatasheetLink,
                    StoreLink = i.StoreLink,
                    Tags = i.Tags,
                    Description = i.Description,
                    PhotoUrl = i.PhotoUrl,
                    IsActive = i.IsActive,
                    Type = i.Type
                })
                .ToListAsync(cancellationToken);
        }
    }
}
