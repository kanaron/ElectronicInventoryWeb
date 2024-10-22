﻿
namespace ElectronicInventoryWeb.Server.Data;

public class InventoryItem
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string DatasheetLink { get; set; } = string.Empty;
    public string StoreLink { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}