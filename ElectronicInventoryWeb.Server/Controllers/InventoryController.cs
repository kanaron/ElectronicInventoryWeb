using ElectronicInventoryWeb.Server.Data;
using ElectronicInventoryWeb.Server.Dto;
using ElectronicInventoryWeb.Server.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicInventoryWeb.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public InventoryController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<InventoryItemDto>> GetInventoryItem(int id)
    {
        var item = await _appDbContext.InventoryItems.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item.ToInventoryItemDto());
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<InventoryItemDto>> AddInventoryItem([FromBody] InventoryItemDto itemDto)
    {
        if (itemDto == null)
        {
            return BadRequest("Item can not be null");
        }

        var item = itemDto.ToInventoryItem();

        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        item.UserId = userId;

        _appDbContext.InventoryItems.Add(item);
        await _appDbContext.SaveChangesAsync();

        return Ok(CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, item));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        var itemsList = await _appDbContext.InventoryItems.Where(x => x.UserId == userId).ToListAsync();

        return Ok(itemsList.Select(x => x.ToInventoryItemDto()));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<InventoryItemDto>> UpdateInventoryItem([FromRoute] int id, [FromBody] UpdateInventoryItemDto inventoryItemDto)
    {
        var item = _appDbContext.InventoryItems.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        item.Type = inventoryItemDto.Type;
        item.Symbol = inventoryItemDto.Symbol;
        item.Category = inventoryItemDto.Category;
        item.Value = inventoryItemDto.Value;
        item.Package = inventoryItemDto.Package;
        item.Quantity = inventoryItemDto.Quantity;
        item.DatasheetLink = inventoryItemDto.DatasheetLink;
        item.StoreLink = inventoryItemDto.StoreLink;
        item.Description = inventoryItemDto.Description;
        

        await _appDbContext.SaveChangesAsync();

        return Ok(item.ToInventoryItemDto());
    }
}
