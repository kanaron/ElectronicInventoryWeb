using Application.Services;
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
        private readonly IReservationService _reservationService;

        public Handler(AppDbContext context, IReservationService reservationService)
        {
            _appDbContext = context;
            _reservationService = reservationService;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _reservationService.RemoveReservationsForProjectAsync(request.ProjectId, cancellationToken);

            var itemToDelete = await _appDbContext.Projects.FindAsync([request.ProjectId], cancellationToken: cancellationToken);
            if (itemToDelete != null)
            {
                _appDbContext.Projects.Remove(itemToDelete);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
