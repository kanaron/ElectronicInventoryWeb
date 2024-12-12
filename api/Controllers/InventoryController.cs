using API.Service;
using Application.InventoryItems;
using Domain.Dto;
using Domain.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class InventoryController : BaseApiController
{
    private readonly TmeApiService _tmeApiService;

    public InventoryController(TmeApiService tmeApiService)
    {
        _tmeApiService = tmeApiService;
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems(CancellationToken cancellationToken)
    {
        var userId = "2b293426-e048-46d6-8be3-faa84664a642";// User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        return await Mediator.Send(new InventoryItemList.Query { UserId = userId }, cancellationToken);
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<InventoryItemDto>> GetInventoryItem(int id, CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        var item = await Mediator.Send(new InventoryItemDetails.Query { ItemId = id }, cancellationToken);

        if (item == null)
        {
            return NotFound();
        }
        else if (item.UserId != userId)
        {
            return Unauthorized("Accessing item of different user");
        }

        return Ok(item.ToInventoryItemDto());
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<InventoryItemDto>> AddInventoryItem([FromBody] InventoryItemDto itemDto, CancellationToken cancellationToken)
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

        await Mediator.Send(new AddInventoryItem.Command { Item = item }, cancellationToken);

        var savedItem = item.ToInventoryItemDto();

        return Ok(CreatedAtAction(nameof(AddInventoryItem), new { id = savedItem.Id }, savedItem));
    }

    [HttpPut]
    [Route("[action]/{id}")]
    public async Task<ActionResult<InventoryItemDto>> UpdateInventoryItem([FromRoute] int id, [FromBody] UpdateInventoryItemDto inventoryItemDto, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EditInventoryItem.Command { ItemDto = inventoryItemDto, ItemId = id }, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]/{id}")]
    public async Task<ActionResult> DeleteInventoryItem([FromRoute] int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteInventoryItem.Command { ItemId = id }, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
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
