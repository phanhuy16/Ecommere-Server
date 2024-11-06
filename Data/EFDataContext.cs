using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public partial class EFDataContext : IdentityDbContext<User>
{
    public EFDataContext() { }
    public EFDataContext(DbContextOptions<EFDataContext> options) : base(options) { }
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<SubProduct> SubProducts { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Cart> Carts { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
    public virtual DbSet<Promotion> Promotions { get; set; } = null!;
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


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