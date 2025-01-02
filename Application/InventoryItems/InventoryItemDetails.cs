using Domain.Data;
using MediatR;
using Persistence;

namespace Application.InventoryItems;

public class InventoryItemDetails
{
    public class Query : IRequest<InventoryItem>
    {
        public int ItemId { get; set; }
    }

    public class Handler : IRequestHandler<Query, InventoryItem>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<InventoryItem?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _appDbContext.InventoryItems.FindAsync([request.ItemId], cancellationToken: cancellationToken);
        }
    }
}
