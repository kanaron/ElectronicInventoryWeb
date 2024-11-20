using ElectronicInventoryWeb.Server.Data;
using ElectronicInventoryWeb.Server.Dto;
using ElectronicInventoryWeb.Server.Mappers;
using ElectronicInventoryWeb.Server.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicInventoryWeb.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly TmeApiService _tmeApiService;

    public InventoryController(AppDbContext appDbContext, TmeApiService tmeApiService)
    {
        _appDbContext = appDbContext;
        _tmeApiService = tmeApiService;
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

        var savedItem = item.ToInventoryItemDto();

        return Ok(CreatedAtAction(nameof(AddInventoryItem), new { id = savedItem.Id }, savedItem));
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

    [HttpGet("FetchFromTme")]
    public async Task<ActionResult<InventoryItemDto>> FetchFromTme([FromQuery] string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol cannot be empty.");
        }

        var productDescription = await _tmeApiService.GetProductWithDescriptionAsync(symbol);
        var productParameters = await _tmeApiService.GetProductWithParametersAsync(symbol);

        if (productDescription == null || productParameters == null)
        {
            return NotFound($"No product found with symbol '{symbol}' in TME.");
        }

        var inventoryItemDto = InventoryMappers.FromTmeToInventoryItemDto(productDescription, productParameters);

        return Ok(inventoryItemDto);
    }
}
