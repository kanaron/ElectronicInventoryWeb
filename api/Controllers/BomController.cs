using API.Interfaces;
using Application.BomItems;
using Application.InventoryItems;
using Application.Projects;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BomController : BaseApiController
{
    private readonly IBomService _bomService;

    private readonly IReservationService _reservationService;

    public BomController(IBomService bomService, IReservationService reservationService)
    {
        _bomService = bomService;
        _reservationService = reservationService;
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

            await Mediator.Send(new MatchBomItemsWithInventory.Command { ProjectId = project.Id, UserId = userId }, cancellationToken);

            await _reservationService.ApplyExactReservationsAsync(project.Id, userId, cancellationToken);

            return Ok(new { project.Id, project.Name, project.Category });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<IEnumerable<BomItemDto>>> GetBomItems([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        return await Mediator.Send(new BomItemsList.Query { ProjectId = id, UserId = userId }, cancellationToken);
    }

    [HttpPut]
    [Route("[action]")]
    public async Task<ActionResult<IEnumerable<BomItemDto>>> UpdateBomItems([FromBody] List<BomItemDto> bomItems, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EditBomItems.Command { BomItems = bomItems }, cancellationToken);

        await _reservationService.UpdateReservationsAsync(bomItems, cancellationToken);

        return Ok();
    }
}
