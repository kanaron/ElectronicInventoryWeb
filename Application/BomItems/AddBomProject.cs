using Domain.Data;
using MediatR;
using Persistence;

namespace Application.BomItems;

public class AddBomProject
{
    public class Command : IRequest
    {
        public Project Project { get; set; }
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
            _appDbContext.Projects.Add(request.Project);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
