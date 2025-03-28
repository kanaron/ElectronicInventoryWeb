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

            return itemsList.Select(x => x.ToBomDto()).ToList();
        }
    }
}
