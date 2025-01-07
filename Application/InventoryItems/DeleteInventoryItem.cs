using Domain.Data;
using MediatR;
using Persistence;

namespace Application.InventoryItems;

public class DeleteInventoryItem
{
    public class Command : IRequest
    {
        public Guid ItemId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        async Task IRequestHandler<Command>.Handle(Command request, CancellationToken cancellationToken)
        {
            var itemToDelete = (await _appDbContext.InventoryItems.FindAsync([request.ItemId], cancellationToken: cancellationToken));

            _appDbContext.Remove(itemToDelete);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
