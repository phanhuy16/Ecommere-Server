

using System.Text.Json.Serialization;

namespace Server.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public string? Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<Category>? SubCategories { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Supplier>? Suppliers { get; set; } = null!;
}