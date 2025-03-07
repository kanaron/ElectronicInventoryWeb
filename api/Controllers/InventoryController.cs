using API.Service;
using Application.InventoryItems;
using Domain.Dto;
using Domain.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace API.Controllers;

public class InventoryController : BaseApiController
{
    private readonly TmeApiService _tmeApiService;

    public InventoryController(TmeApiService tmeApiService)
    {
        _tmeApiService = tmeApiService;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems(CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

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

        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        var item = itemDto.ToInventoryItem();
        item.UserId = userId;

        var existingItem = await Mediator.Send(new ItemDuplicateFind.Query
        {
            UserId = userId,
            Type = item.Type,
            Symbol = item.Symbol,
            Value = item.Value,
            Package = item.Package
        }, cancellationToken);

        if (existingItem != null)
        {
            var existingItemDto = existingItem.ToUpdateInventoryItemDto();
            existingItemDto.Quantity += item.Quantity;
            existingItemDto.IsActive = true;
            if (existingItemDto.Location != item.Location)
            {
                existingItemDto.Location = $"{existingItemDto.Location} | {item.Location}";
            }
            await Mediator.Send(new EditInventoryItem.Command { ItemDto = existingItemDto, ItemId = existingItemDto.Id }, cancellationToken);

            return Ok(existingItemDto);
        }


        await Mediator.Send(new AddInventoryItem.Command { Item = item }, cancellationToken);

        return Created();
    }

    [HttpPut]
    [Route("[action]/{id}")]
    public async Task<ActionResult<InventoryItemDto>> UpdateInventoryItem([FromRoute] Guid id, [FromBody] UpdateInventoryItemDto inventoryItemDto, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EditInventoryItem.Command { ItemDto = inventoryItemDto, ItemId = id }, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]/{id}")]
    public async Task<ActionResult> DeleteInventoryItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteInventoryItem.Command { ItemId = id }, cancellationToken);

        return Ok();
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

    [HttpGet("FetchFromTmeQrCode")]
    public async Task<ActionResult<InventoryItemDto>> FetchFromTmeQrCode([FromQuery] string qrCode)
    {
        if (string.IsNullOrWhiteSpace(qrCode))
        {
            return BadRequest("Symbol cannot be empty.");
        }

        string pattern = @"QTY:(\d+)\s+PN:([^\s]+)(?=\s+PO:)";
        var match = Regex.Match(qrCode, pattern);

        if (match.Success)
        {
            var qty = int.Parse(match.Groups[1].Value);
            var symbol = match.Groups[2].Value;

            var productDescription = await _tmeApiService.GetProductWithDescriptionAsync(symbol);
            var productParameters = await _tmeApiService.GetProductWithParametersAsync(symbol);

            if (productDescription == null || productParameters == null)
            {
                return NotFound($"No product found with symbol '{symbol}' in TME.");
            }

            var inventoryItemDto = InventoryMappers.FromTmeToInventoryItemDto(productDescription, productParameters);

            inventoryItemDto.Quantity = qty;

            return Ok(inventoryItemDto);
        }

        return BadRequest("Invalid QR code");
    }
}
