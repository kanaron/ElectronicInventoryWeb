using Microsoft.AspNetCore.Identity;

namespace Domain.Data;

public class User : IdentityUser
{
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; set; }
    public ICollection<Project> Projects { get; set; }

    public string? TmeToken { get; set; }
}
