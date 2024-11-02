

using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<SubProduct> SubProducts { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = null!;

}