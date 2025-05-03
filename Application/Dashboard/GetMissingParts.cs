using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Dashboard;

public class GetMissingParts
{
    public class Query : IRequest<List<BomItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Query, List<BomItemDto>>
    {
        private readonly AppDbContext _context;
        public Handler(AppDbContext context) => _context = context;

        public async Task<List<BomItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var bomItems = await _context.BomItems
                .Include(b => b.Project)
                .Where(b =>
                    b.Project!.UserId == request.UserId &&
                    b.IsRelevant &&
                    !b.IsPlaced &&
                    b.SelectedInventoryItemIds.Any())
                .ToListAsync(cancellationToken);

            var inventoryItems = await _context.InventoryItems
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            var missingItems = new List<BomItemDto>();

            foreach (var bom in bomItems)
            {
                int available = bom.SelectedInventoryItemIds
                    .Select(id => inventoryItems.FirstOrDefault(i => i.Id == id))
                    .Where(i => i != null)
                    .Sum(i => i!.Quantity - i.ReservedForProjects);

                if (available < bom.Quantity)
                {
                    missingItems.Add(new BomItemDto
                    {
                        Id = bom.Id,
                        Category = bom.Category,
                        Value = bom.Value,
                        Package = bom.Package,
                        Quantity = bom.Quantity,
                        Description = bom.Description,
                        References = bom.References,
                        IsPlaced = bom.IsPlaced,
                        IsRelevant = bom.IsRelevant,
                        MatchingInventoryItemIds = bom.MatchingInventoryItemIds,
                        SelectedInventoryItemIds = bom.SelectedInventoryItemIds
                    });
                }
            }

            return missingItems;
        }
    }
}
