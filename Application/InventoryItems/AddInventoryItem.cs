using Domain.Data;
using MediatR;
using Persistence;

namespace Application.InventoryItems;

public class AddInventoryItem
{
    public class Command : IRequest
    {
        public InventoryItem Item { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }


        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _appDbContext.InventoryItems.Add(request.Item);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
