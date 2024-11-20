using ElectronicInventoryWeb.Server.Data;
using ElectronicInventoryWeb.Server.Dto;

namespace ElectronicInventoryWeb.Server.Mappers;

public static class InventoryMappers
{
    public static InventoryItemDto ToInventoryItemDto(this InventoryItem item)
    {
        return new InventoryItemDto
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = item.Value,
            Package = item.Package,
            Quantity = item.Quantity,
            Location = item.Location,
            DatasheetLink = item.DatasheetLink,
            StoreLink = item.StoreLink,
            PhotoUrl = item.PhotoUrl,
            MinStockLevel = item.MinStockLevel,
            Description = item.Description,
            IsActive = item.IsActive,
            Tags = item.Tags,
            DateAdded = item.DateAdded,
            LastUpdated = item.LastUpdated
        };
    }

    public static InventoryItem ToInventoryItem(this InventoryItemDto item)
    {
        return new InventoryItem
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = item.Value,
            Package = item.Package,
            Quantity = item.Quantity,
            Location = item.Location,
            DatasheetLink = item.DatasheetLink,
            StoreLink = item.StoreLink,
            PhotoUrl = item.PhotoUrl,
            MinStockLevel = item.MinStockLevel,
            Description = item.Description,
            IsActive = item.IsActive,
            Tags = item.Tags,
            DateAdded = item.DateAdded,
            LastUpdated = item.LastUpdated
        };
    }
}
