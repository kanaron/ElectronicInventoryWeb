using Domain.Data;
using Domain.Dto;

namespace Domain.Mappers;

public static class ProjectMappers
{
    public static ProjectDto ToProjectDto(this Project project)
    {
        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            Category = project.Category,
            Description = project.Description,
            IsFinished = project.IsFinished
        };
    }

    public static Project ToProject(this ProjectDto project) 
    {
        return new Project()
        {
            Id = project.Id,
            Name = project.Name,
            Category = project.Category,
            Description = project.Description,
            IsFinished = project.IsFinished
        };
    }
}
