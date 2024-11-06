

using System.Text.Json.Serialization;

namespace Server.Entities;


public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[]? Images { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    [JsonIgnore]
    public virtual ICollection<SubProduct> SubProducts { get; set; } = new List<SubProduct>();
    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    [JsonIgnore]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}