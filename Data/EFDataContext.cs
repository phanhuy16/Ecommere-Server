using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public partial class EFDataContext : IdentityDbContext<User>
{
    public EFDataContext() { }
    public EFDataContext(DbContextOptions<EFDataContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<SubProduct> SubProducts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }

        // Add Role
        var user = new IdentityRole("User");
        user.NormalizedName = "USER";
        var admin = new IdentityRole("Admin");
        admin.NormalizedName = "ADMIN";

        modelBuilder.Entity<IdentityRole>().HasData(user);
        modelBuilder.Entity<IdentityRole>().HasData(admin);

        // Cấu hình cho SubProduct
        modelBuilder.Entity<SubProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_subproducts_id");
            entity.ToTable("SubProducts");

            entity.HasOne(sp => sp.Product)
                .WithMany(p => p.SubProducts)
                .HasForeignKey(sp => sp.Product_Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sp => sp.Order)
                .WithMany(o => o.SubProducts)
                .HasForeignKey(p => p.Order_Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình cho Promotion
        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(p => p.Id).HasName("PK_promotion_id");
            entity.ToTable("Promotion");
        });

        // Cấu hình cho ProductCategory
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.CategoryId }) // Khóa chính
                .HasName("PK_ProductCategory");

            entity.ToTable("ProductCategories"); // Tên bảng

            entity.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình cho Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_products_id");
            entity.ToTable("Products");

            entity.HasMany(p => p.ProductCategories) // Chỉ định quan hệ nhiều-nhiều
                .WithOne(pc => pc.Product) // Đối tượng thuộc ProductCategory
                .HasForeignKey(pc => pc.ProductId);

            entity.HasMany(p => p.SubProducts)
                .WithOne(sp => sp.Product)
                .HasForeignKey(sp => sp.Product_Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình cho Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(e => e.Id);

            entity.HasMany(c => c.Suppliers)
                .WithOne(c => c.category)
                .HasForeignKey(c => c.id)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình cho ProductCategories
            entity.HasMany(c => c.ProductCategories)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.CategoryId);

            entity.HasMany(c => c.SubCategories)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("carts");
            entity.HasKey(s => s.Cart_Id);

            entity.HasOne(c => c.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(c => c.User_Id)
                .OnDelete(DeleteBehavior.Restrict);

        });

        //Order
        modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(s => s.Order_Id);
            });

        //User 
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
        });

        // Cấu hình cho Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.id).HasName("PK_supplier_id");
            entity.ToTable("Suppliers");

            entity.HasOne(s => s.category)
                .WithMany(s => s.Suppliers)
                .HasForeignKey(s => s.category_id)
                .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}