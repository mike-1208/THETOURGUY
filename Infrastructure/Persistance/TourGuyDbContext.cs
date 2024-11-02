using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistance;

public class TourGuyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;

    public TourGuyDbContext(DbContextOptions<TourGuyDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId);
    }
}