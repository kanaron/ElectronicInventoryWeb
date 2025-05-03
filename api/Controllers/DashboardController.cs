using Application.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DashboardController : BaseApiController
{
    public DashboardController()
    {
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> MissingParts(CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId == null) return Unauthorized();

        var result = await Mediator.Send(new GetMissingParts.Query { UserId = userId }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> LowStock(CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId == null) return Unauthorized();

        var result = await Mediator.Send(new GetLowStockItems.Query { UserId = userId }, cancellationToken);
        return Ok(result);
    }
}
