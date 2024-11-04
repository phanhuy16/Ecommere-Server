

using System.Text.Json.Serialization;

namespace Server.Entities;

public class SubProduct
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Qty { get; set; } = 0;
    public int Discount { get; set; }
    public string[]? Images { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign key to Product
    public Guid ProductId { get; set; }
    public Guid? OrderId { get; set; }

    [JsonIgnore]
    public virtual Order? Order { get; set; } = null!;
    [JsonIgnore]
    public virtual Product? Product { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Cart>? Carts { get; set; } = null!;
}