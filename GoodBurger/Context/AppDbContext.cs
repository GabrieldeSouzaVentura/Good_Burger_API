using GoodBurger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Burger>? Burgers { get; set; }
    public DbSet<Extra>? Extras { get; set; }
    public DbSet<Order>? Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Burger>().HasData(
            new Burger { BurgerId = 1, Name = "X Burger", Price = 5.00 },
            new Burger { BurgerId = 2, Name = "X Bacon", Price = 7.00 },
            new Burger { BurgerId = 3, Name = "X Egg", Price = 4.50 }
        );

        modelBuilder.Entity<Extra>().HasData(
            new Extra { ExtraId = 1, Name = "French Fries", Price = 2.00 },
            new Extra { ExtraId = 2, Name = "Soda", Price = 2.50 }
        );

        modelBuilder.Entity<Order>().HasMany(p => p.Extras).WithMany().UsingEntity(j => j.ToTable("ExtraOrder"));
    }
}
