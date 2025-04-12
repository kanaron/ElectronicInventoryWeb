using Domain.Data;
using Domain.Dto;

namespace Domain.Mappers;

public static class BomMappers
{
    public static BomItemDto ToBomDto(this BomItem item)
    {
        return new BomItemDto()
        {
            Id = item.Id,
            Value = item.Value,
            Category = item.Category,
            Description = item.Description,
            Package = item.Package,
            Quantity = item.Quantity,
            References = item.References,
            IsPlaced = item.IsPlaced,
            IsRelevant = item.IsRelevant,
            MatchingInventoryItemIds = item.MatchingInventoryItemIds,
            SelectedInventoryItemIds = item.SelectedInventoryItemIds,
            IsMatched = item.IsMatched,
        };
    }

    public static BomItem ToBom(this BomItemDto item) 
    {
        return new BomItem()
        {
            Id = item.Id,
            Value = item.Value,
            Category = item.Category,
            Description = item.Description,
            Package = item.Package,
            Quantity = item.Quantity,
            References = item.References,
            IsPlaced = item.IsPlaced,
            IsRelevant = item.IsRelevant,
            MatchingInventoryItemIds = item.MatchingInventoryItemIds,
            SelectedInventoryItemIds = item.SelectedInventoryItemIds,
            IsMatched = item.IsMatched,
        };
    }
}
