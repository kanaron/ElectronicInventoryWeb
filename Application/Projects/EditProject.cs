using Application.Services;
using Domain.Dto;
using MediatR;
using Persistence;

namespace Application.Projects;

public class EditProject
{
    public class Command : IRequest
    {
        public ProjectDto ProjectDto { get; set; }
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
            var projectToEdit = await _appDbContext.Projects.FindAsync(
                new object[] { request.ProjectId },
                cancellationToken: cancellationToken
            );

            if (projectToEdit == null)
                return;

            bool wasFinished = projectToEdit.IsFinished;
            bool willBeFinished = request.ProjectDto.IsFinished;

            projectToEdit.Name = request.ProjectDto.Name;
            projectToEdit.Category = request.ProjectDto.Category;
            projectToEdit.Description = request.ProjectDto.Description;
            projectToEdit.IsFinished = willBeFinished;

            if (!wasFinished && willBeFinished)
            {
                await _reservationService.RemoveReservationsForProjectAsync(request.ProjectId, cancellationToken);
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
