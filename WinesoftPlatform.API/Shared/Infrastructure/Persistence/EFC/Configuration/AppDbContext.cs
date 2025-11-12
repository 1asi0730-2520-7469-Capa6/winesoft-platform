using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using WinesoftPlatform.API.Orders.Domain.Model.Aggregates;
using WinesoftPlatform.API.Inventory.Domain.Model.Aggregates;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Automatically set CreatedDate and UpdatedDate for entities
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Order>().ToTable("orders");
        builder.Entity<Order>().HasKey(o => o.Id);
        builder.Entity<Order>().Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Order>().Property(o => o.ProductId).IsRequired();
        builder.Entity<Order>().Property(o => o.Supplier).IsRequired().HasMaxLength(100);
        builder.Entity<Order>().Property(o => o.Quantity).IsRequired();
        builder.Entity<Order>().Property(o => o.Status).IsRequired().HasMaxLength(30);
        builder.Entity<Order>().Property(o => o.CreatedDate);
        builder.Entity<Order>().Property(o => o.UpdatedDate);

        // Inventory Bounded Context configurations
        builder.Entity<Supply>().HasKey(s => s.Id);
        builder.Entity<Supply>().Property(s => s.SupplyName).IsRequired();
        builder.Entity<Supply>().Property(s => s.Quantity).IsRequired();
        builder.Entity<Supply>().Property(s => s.Unit).IsRequired();
        builder.Entity<Supply>().Property(s => s.Price).IsRequired();
        
        // Apply naming convention to use snake_case for database objects
        builder.UseSnakeCaseNamingConvention();
    }
}