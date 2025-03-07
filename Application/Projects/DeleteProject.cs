using Domain.Data;
using MediatR;
using Persistence;

namespace Application.Projects;

public class DeleteProject
{
    public class Command : IRequest
    {
        public Guid ProjectId { get; set; }
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
            var itemToDelete = await _appDbContext.Projects.FindAsync([request.ProjectId], cancellationToken: cancellationToken);

            _appDbContext.Remove(itemToDelete);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
