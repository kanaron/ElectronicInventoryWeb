using Domain.Dto;
using Domain.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.InventoryItems;

public class InventoryItemList
{
    public class Query : IRequest<List<InventoryItemDto>>
    {
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<InventoryItemDto>>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<List<InventoryItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var itemsList = await _appDbContext.InventoryItems
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);

            return itemsList.Select(x => x.ToInventoryItemDto()).ToList();
        }
    }
}
