namespace Domain.Data;

public class BomItemReservation
{
    public Guid Id { get; set; }
    public Guid BomItemId { get; set; }
    public Guid InventoryItemId { get; set; }
    public int ReservedQuantity { get; set; }

    public BomItem? BomItem { get; set; }
    public InventoryItem? InventoryItem { get; set; }
}
