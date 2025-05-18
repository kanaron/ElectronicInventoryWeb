using Domain.Data;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ApplyExactReservationsAsync(Guid projectId, string userId, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.BomItems)
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);

        if (project == null) return;

        var inventory = await _context.InventoryItems
            .Where(i => i.UserId == userId)
            .ToListAsync(cancellationToken);

        foreach (var bom in project.BomItems)
        {
            if (!bom.IsRelevant || bom.SelectedInventoryItemIds.Count == 0)
                continue;

            foreach (var invId in bom.SelectedInventoryItemIds)
            {
                var inventoryItem = inventory.FirstOrDefault(i => i.Id == invId);
                if (inventoryItem == null) continue;

                var reservation = new BomItemReservation
                {
                    Id = Guid.NewGuid(),
                    BomItemId = bom.Id,
                    InventoryItemId = invId,
                    ReservedQuantity = bom.Quantity
                };

                inventoryItem.ReservedForProjects += bom.Quantity;
                _context.BomItemReservations.Add(reservation);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateReservationsAsync(List<BomItemDto> updatedBomItems, CancellationToken cancellationToken)
    {
        foreach (var bom in updatedBomItems)
        {
            // Remove old reservations
            var oldReservations = await _context.BomItemReservations
                .Where(r => r.BomItemId == bom.Id)
                .ToListAsync(cancellationToken);

            foreach (var res in oldReservations)
            {
                var inv = await _context.InventoryItems.FindAsync(res.InventoryItemId);
                if (inv != null)
                {
                    inv.ReservedForProjects -= res.ReservedQuantity;
                }
            }

            _context.BomItemReservations.RemoveRange(oldReservations);

            int remaining = bom.Quantity;

            var selectedIds = bom.SelectedInventoryItemIds;
            if (selectedIds.Count == 1)
            {
                // Only one selected, assign entire reservation even if overbooking
                var inv = await _context.InventoryItems.FindAsync(selectedIds[0]);
                if (inv == null) continue;

                var reservation = new BomItemReservation
                {
                    Id = Guid.NewGuid(),
                    BomItemId = bom.Id,
                    InventoryItemId = inv.Id,
                    ReservedQuantity = remaining
                };

                inv.ReservedForProjects += remaining;
                _context.BomItemReservations.Add(reservation);
            }
            else
            {
                for (int i = 0; i < selectedIds.Count; i++)
                {
                    var invId = selectedIds[i];
                    var inv = await _context.InventoryItems.FindAsync(invId);
                    if (inv == null) continue;

                    int toReserve;

                    if (i < selectedIds.Count - 1)
                    {
                        // Try to reserve within available stock
                        toReserve = Math.Min(remaining, inv.Quantity - inv.ReservedForProjects);
                        if (toReserve <= 0) continue;
                    }
                    else
                    {
                        // Last in priority gets the rest (even if overbooking)
                        toReserve = remaining;
                    }

                    var reservation = new BomItemReservation
                    {
                        Id = Guid.NewGuid(),
                        BomItemId = bom.Id,
                        InventoryItemId = inv.Id,
                        ReservedQuantity = toReserve
                    };

                    inv.ReservedForProjects += toReserve;
                    _context.BomItemReservations.Add(reservation);

                    remaining -= toReserve;
                    if (remaining <= 0) break;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveReservationsForProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var reservations = await _context.BomItemReservations
            .Where(r => r.BomItem.ProjectId == projectId)
            .ToListAsync(cancellationToken);

        foreach (var res in reservations)
        {
            var inv = await _context.InventoryItems.FindAsync(res.InventoryItemId);
            if (inv != null)
            {
                inv.ReservedForProjects -= res.ReservedQuantity;
            }
        }

        _context.BomItemReservations.RemoveRange(reservations);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
