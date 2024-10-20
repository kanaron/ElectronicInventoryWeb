
namespace ElectronicInventoryWeb.Server.Data;

public class InventoryItem
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string? Symbol { get; set; }
    public string? Category { get; set; }
    public string Value { get; set; }
    public string Package { get; set; }
    public int Quantity { get; set; }
    public string DatasheetLink { get; set; }
    public string StoreLink { get; set; }
    public string? Description { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;

    public string UserId { get; set; }
    public User User { get; set; }
}