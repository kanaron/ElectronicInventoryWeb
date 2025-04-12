using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BomItems;

public class MatchBomItemsWithInventory
{
    public class Command : IRequest
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.BomItems)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                throw new Exception("Project not found");

            var inventory = await _context.InventoryItems
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            var reservations = new Dictionary<Guid, int>();

            foreach (var bom in project.BomItems)
            {
                if (!bom.IsRelevant)
                    continue;

                var matches = inventory
                    .Where(inv =>
                        inv.Category.Equals(bom.Category, StringComparison.OrdinalIgnoreCase) &&
                        inv.Package.Equals(bom.Package, StringComparison.OrdinalIgnoreCase) &&
                        AreValuesClose(inv.StandardValue, bom.StandardValue))
                    .ToList();

                if (matches.Any())
                {
                    bom.IsMatched = true;
                    bom.MatchingInventoryItemIds = matches.Select(m => m.Id).ToList();

                    foreach (var match in matches)
                    {
                        if (!reservations.ContainsKey(match.Id))
                            reservations[match.Id] = 0;

                        reservations[match.Id] += bom.Quantity;
                    }

                    if (bom.Category.Equals("Light emitting diode", StringComparison.OrdinalIgnoreCase))
                    {
                        bom.IsMatched = false;
                    }
                }
                else
                {
                    bom.IsMatched = false;

                    var sameCategoryFallback = inventory
                        .Where(inv => inv.Category.Equals(bom.Category, StringComparison.OrdinalIgnoreCase))
                        .Select(inv => inv.Id);

                    var otherCategoryFallback = inventory
                        .Where(inv => inv.Category.Equals("Other", StringComparison.OrdinalIgnoreCase))
                        .Select(inv => inv.Id);

                    bom.MatchingInventoryItemIds = sameCategoryFallback
                        .Concat(otherCategoryFallback)
                        .Distinct()
                        .ToList();
                }
            }

            foreach (var kvp in reservations)
            {
                var inventoryItem = inventory.FirstOrDefault(i => i.Id == kvp.Key);
                if (inventoryItem != null)
                {
                    inventoryItem.ReservedForProjects += kvp.Value;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private bool AreValuesClose(double a, double b, double tolerancePercent = 0.01)
        {
            if (a == 0 || b == 0) return a == b;
            return Math.Abs(a - b) <= a * tolerancePercent;
        }
    }
}
