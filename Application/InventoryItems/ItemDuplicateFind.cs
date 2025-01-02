using Domain.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.InventoryItems;

public class ItemDuplicateFind
{
    public class Query : IRequest<InventoryItem>
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public string Value { get; set; }
        public string Package { get; set; }
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
            return await _appDbContext.InventoryItems
                .FirstOrDefaultAsync(i =>
                    i.UserId == request.UserId &&
                    i.Type == request.Type &&
                    i.Symbol == request.Symbol &&
                    i.Value == request.Value &&
                    i.Package == request.Package,
                    cancellationToken);
        }
    }
}
