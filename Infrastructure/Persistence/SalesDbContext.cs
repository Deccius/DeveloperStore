using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace Sales.Infrastructure.Persistence;
public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {

    }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map Sale and its items
        modelBuilder.Entity<Sale>(b =>
        {
            b.HasKey(nameof(Sale.Id));
            b.Property<Guid>("Id");
            b.Property(s => s.SaleNumber).IsRequired();
            b.Property(s => s.Date).IsRequired();
            b.Property(s => s.CustomerId).IsRequired();
            b.Property(s => s.CustomerName).IsRequired();
            b.Property(s => s.BranchId).IsRequired();
            b.Property(s => s.BranchName).IsRequired();
            b.Property(s => s.Cancelled).IsRequired();
            b.OwnsMany<SaleItem>("Items", a =>
            {
                a.WithOwner().HasForeignKey("SaleId");
                a.Property<Guid>("Id");
                a.HasKey("Id");
                a.Property<string>("ProductId");
                a.Property<string>("ProductName");
                a.Property<int>("Quantity");
                a.Property<decimal>("UnitPrice");
                a.Property<decimal>("DiscountPercentage");
                a.Property<bool>("Cancelled");
            })
            .Navigation("Items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
            b.Ignore("Events");
        });
    }
}