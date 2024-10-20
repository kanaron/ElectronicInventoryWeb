using ElectronicInventoryWeb.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicInventoryWeb.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddInventoryController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public AddInventoryController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
    {
        var item = await _appDbContext.InventoryItems.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<InventoryItem>> AddInventoryItem(InventoryItem item)
    {
        if (item == null)
        {
            return BadRequest("Item can not be null");
        }

        _appDbContext.InventoryItems.Add(item);
        await _appDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, item);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        return await _appDbContext.InventoryItems.ToListAsync();
    }
}
