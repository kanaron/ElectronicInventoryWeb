namespace Domain.Dto;

public class BomItemDto
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public string[]? References { get; set; } = [];
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public bool IsRelevant { get; set; } = true;
    public bool IsPlaced { get; set; } = false;

    public List<Guid> MatchingInventoryItemIds { get; set; } = [];
    public List<InventoryItemDto> MatchingItems { get; set; } = [];

    public List<Guid> SelectedInventoryItemIds { get; set; } = [];

    public bool IsMatched { get; set; } = false;
}
