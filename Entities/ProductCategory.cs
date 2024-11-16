using System.Text.Json.Serialization;

namespace Server.Entities;


public class ProductCategory
{
    public Guid ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; } = null!;

    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; } = null!;
}