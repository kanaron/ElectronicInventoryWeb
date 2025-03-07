using Application.Projects;
using Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProjectController : BaseApiController
{
    public ProjectController()
    {
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects(CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        return await Mediator.Send(new ProjectsList.Query { UserId = userId }, cancellationToken);
    }

    [HttpPut]
    [Route("[action]/{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject([FromRoute] Guid id, [FromBody] ProjectDto projectDto, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EditProject.Command { ProjectDto = projectDto, ProjectId = id }, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]/{id}")]
    public async Task<ActionResult> DeleteProject([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProject.Command { ProjectId = id }, cancellationToken);

        return Ok();
    }
}
