﻿namespace Domain.Data;

public class InventoryItem
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double StandardValue { get; set; }
    public string StandardUnit { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReservedForProjects { get; set; }
    public string Location { get; set; } = "Default";
    public string DatasheetLink { get; set; } = string.Empty;
    public string StoreLink { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public int MinStockLevel { get; set; } = 0;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public string[]? Tags { get; set; } = Array.Empty<string>();
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public DateTime LastUpdated { get; set; } = DateTime.Now;

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}