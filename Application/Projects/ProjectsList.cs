using Domain.Dto;
using Domain.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Projects;

public class ProjectsList
{
    public class Query : IRequest<List<ProjectDto>>
    {
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<ProjectDto>>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var projectsList = await _appDbContext.Projects
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);

            return projectsList.Select(x => x.ToProjectDto()).ToList();
        }
    }
}
