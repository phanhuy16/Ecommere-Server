

namespace Server.Entities;


public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[]? Images { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; } = new HashSet<ProductCategory>();
    public virtual ICollection<SubProduct>? SubProducts { get; set; } = new HashSet<SubProduct>(); // Cập nhật
    public virtual ICollection<Cart>? Carts { get; set; } = new HashSet<Cart>(); // Cập nhật

    public Product()
    {
        ProductCategories = new HashSet<ProductCategory>();
        SubProducts = new HashSet<SubProduct>();
        Carts = new HashSet<Cart>();
    }
}