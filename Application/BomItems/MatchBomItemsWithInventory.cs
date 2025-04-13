using Domain.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Text.RegularExpressions;

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
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken)
                ?? throw new Exception("Project not found");

            var inventory = await _context.InventoryItems
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            var reservations = new Dictionary<Guid, int>();

            foreach (var bom in project.BomItems)
            {
                if (!bom.IsRelevant)
                    continue;

                List<Guid> matchedIds;
                List<InventoryItem> matches;

                if (bom.Category.Equals("Light emitting diode", StringComparison.OrdinalIgnoreCase))
                {
                    matches = inventory
                        .Where(inv =>
                            inv.Category.Equals(bom.Category, StringComparison.OrdinalIgnoreCase) &&
                            inv.Package.Equals(bom.Package, StringComparison.OrdinalIgnoreCase) &&
                            inv.Value.Equals(bom.Value, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else
                {
                    matches = inventory
                        .Where(inv =>
                            inv.Category.Equals(bom.Category, StringComparison.OrdinalIgnoreCase) &&
                            inv.Package.Equals(bom.Package, StringComparison.OrdinalIgnoreCase) &&
                            AreValuesClose(inv.StandardValue, bom.StandardValue))
                        .ToList();
                }

                matchedIds = matches.Select(m => m.Id).ToList();
                bom.IsMatched = matches.Count > 0;

                if (matches.Count == 1)
                {
                    if (!reservations.ContainsKey(matches.First().Id))
                        reservations[matches.First().Id] = 0;

                    reservations[matches.First().Id] += bom.Quantity;
                }

                if (!bom.IsMatched)
                {
                    // Fallback to same-category and "Other"
                    var sameCategoryFallback = inventory
                        .Where(inv => inv.Category.Equals(bom.Category, StringComparison.OrdinalIgnoreCase))
                        .Select(inv => inv.Id);

                    var otherCategoryFallback = inventory
                        .Where(inv => inv.Category.Equals("Other", StringComparison.OrdinalIgnoreCase))
                        .Select(inv => inv.Id);

                    matchedIds = sameCategoryFallback
                        .Concat(otherCategoryFallback)
                        .Distinct()
                        .ToList();
                }

                bom.MatchingInventoryItemIds = matchedIds;

                if (bom.IsMatched && matchedIds.Count == 1)
                {
                    bom.SelectedInventoryItemIds = [matchedIds[0]];
                }
                else
                {
                    bom.SelectedInventoryItemIds = [];
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

        private static bool AreValuesClose(double a, double b, double tolerancePercent = 0.01)
        {
            if (a == 0 || b == 0) return a == b;
            return Math.Abs(a - b) <= a * tolerancePercent;
        }
    }
}
