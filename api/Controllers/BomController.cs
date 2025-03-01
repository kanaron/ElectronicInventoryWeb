using API.Interfaces;
using Application.BomItems;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BomController : BaseApiController
{
    private readonly IBomService _bomService;

    public BomController(IBomService bomService)
    {
        _bomService = bomService;
    }

    [HttpPost("[action]")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProjectDto>> UploadBomFile(IFormFile file, [FromForm] string projectName, [FromForm] string category, 
        [FromForm] string? description, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        try
        {
            var project = await _bomService.ProcessBomFileAsync(file, projectName, category, description);
            project.UserId = userId;

            await Mediator.Send(new AddBomProject.Command { Project = project }, cancellationToken);
            return Ok(new { project.Id, project.Name, project.Category });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
