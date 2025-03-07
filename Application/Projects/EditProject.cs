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

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }


        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var projectToEdit = await _appDbContext.Projects.FindAsync([request.ProjectId], cancellationToken: cancellationToken);

            if (projectToEdit == null)
            {
                return;
            }

            projectToEdit.Name = request.ProjectDto.Name;
            projectToEdit.Category = request.ProjectDto.Category;
            projectToEdit.Description = request.ProjectDto.Description;
            projectToEdit.IsFinished = request.ProjectDto.IsFinished;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
