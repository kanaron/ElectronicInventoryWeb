using Domain.Data;
using MediatR;
using Persistence;

namespace Application.InventoryItems;

public class DeleteInventoryItem
{
    public class Command : IRequest
    {
        public int ItemId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<InventoryItem?> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _appDbContext.InventoryItems.FindAsync(request.ItemId);
        }

        async Task IRequestHandler<Command>.Handle(Command request, CancellationToken cancellationToken)
        {
            var itemToDelete = _appDbContext.InventoryItems.FindAsync(request.ItemId).Result;

            _appDbContext.Remove(itemToDelete);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
