using Domain.Dto;
using Domain.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.InventoryItems;

public class BomItemsList
{
    public class Query : IRequest<List<BomItemDto>>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<BomItemDto>>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<List<BomItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var itemsList = await _appDbContext.BomItems
            .Where(x => x.ProjectId == request.ProjectId)
            .ToListAsync(cancellationToken);

            var inventoryItems = await _appDbContext.InventoryItems
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);

            var result = itemsList.Select(x =>
            {
                var dto = x.ToBomDto();
                dto.MatchingItems = inventoryItems
                    .Where(i => dto.MatchingInventoryItemIds.Contains(i.Id))
                    .Select(i => i.ToInventoryItemDto())
                    .ToList();
                return dto;
            }).ToList();

            return result;
        }
    }
}
