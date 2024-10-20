using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ElectronicInventoryWeb.Server.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(u => u.InventoryItems)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);

        builder.Entity<User>()
            .HasOne(u => u.Subscription)
            .WithOne(s => s.User)
            .HasForeignKey<Subscription>(s => s.UserId);

        builder.Entity<Subscription>()
        .Property(s => s.Price)
        .HasPrecision(6, 2);
    }
}
