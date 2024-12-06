namespace Domain.Data;

public class Subscription
{
    public int Id { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public int MaxInventoryItems { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpirationDate { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}