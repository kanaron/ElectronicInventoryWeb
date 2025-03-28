namespace Domain.Data;

public class BomItem
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double StandardValue { get; set; }
    public string StandardUnit { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public string[]? References { get; set; } = [];
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public bool IsRelevant { get; set; } = true;
    public bool IsPlaced { get; set; } = false;

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    public List<Guid> MatchingInventoryItemIds { get; set; } = new();
}