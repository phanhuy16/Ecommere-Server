using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public float Price { get; set; }
    public string Product { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public bool? IsTalking { get; set; } = false;
    public string? Email { get; set; } = string.Empty;
    public int Active { get; set; }
    public string? Image { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual List<SupplierCategory> SupplierCategory { get; set; } = new List<SupplierCategory>();
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}