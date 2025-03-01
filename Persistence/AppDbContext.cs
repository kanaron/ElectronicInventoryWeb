﻿using Domain.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<BomItem> BomItems { get; set; }

    public DbSet<Project> Projects { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        List<IdentityRole> roles = new()
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },
        };

        builder.Entity<IdentityRole>().HasData(roles);

        builder.Entity<User>()
            .HasMany(u => u.InventoryItems)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);

        builder.Entity<User>()
            .HasMany(u => u.Projects)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);

        builder.Entity<Project>()
            .HasMany(p => p.BomItems)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasOne(u => u.Subscription)
            .WithOne(s => s.User)
            .HasForeignKey<Subscription>(s => s.UserId);

        builder.Entity<Subscription>()
        .Property(s => s.Price)
        .HasPrecision(6, 2);
    }
}
