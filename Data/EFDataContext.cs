using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public partial class EFDataContext : IdentityDbContext<User>
{
    public EFDataContext() { }
    public EFDataContext(DbContextOptions<EFDataContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<SubProduct> SubProducts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(6,2)");
        }

        // Add Role
        var user = new IdentityRole("User");
        user.NormalizedName = "USER";
        var admin = new IdentityRole("Admin");
        admin.NormalizedName = "ADMIN";

        modelBuilder.Entity<IdentityRole>().HasData(user);
        modelBuilder.Entity<IdentityRole>().HasData(admin);

        // Cấu hình cho ProductCategory
        modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });

        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("carts");
            entity.HasKey(s => s.Id);

            entity.HasOne(c => c.SubProduct)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.SubProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Products)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        base.OnModelCreating(modelBuilder);
    }
}