﻿using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BomItems;

public class EditBomItems
{
    public class Command : IRequest
    {
        public List<BomItemDto> BomItems { get; set; } = [];
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
            var ids = request.BomItems.Select(b => b.Id).ToList();
            var itemsToUpdate = await _context.BomItems
                .Where(b => ids.Contains(b.Id))
                .ToListAsync(cancellationToken);

            foreach (var dto in request.BomItems)
            {
                var item = itemsToUpdate.FirstOrDefault(x => x.Id == dto.Id);
                if (item is null) continue;

                item.Category = dto.Category;
                item.Value = dto.Value;
                item.Package = dto.Package;
                item.References = dto.References;
                item.Quantity = dto.Quantity;
                item.Description = dto.Description;
                item.IsRelevant = dto.IsRelevant;
                item.IsPlaced = dto.IsPlaced;

                item.MatchingInventoryItemIds = dto.MatchingInventoryItemIds ?? [];
                item.SelectedInventoryItemIds = dto.SelectedInventoryItemIds ?? [];

                var currentGroup = item.IsMatched switch
                {
                    0 or 3 => "none",
                    1 or 4 => "multi",
                    2 => "exact",
                    _ => "unknown"
                };

                item.IsMatched = currentGroup switch
                {
                    "none" => item.SelectedInventoryItemIds.Any() ? 3 : 0,
                    "multi" => item.SelectedInventoryItemIds.Any() ? 4 : 1,
                    "exact" => 2,
                    _ => item.IsMatched 
                };
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

