using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public string Image { get; set; } = string.Empty;
    public Guid SubProductId { get; set; }
    public Guid ProductId { get; set; }

    [JsonIgnore]
    public virtual SubProduct? SubProduct { get; set; } = null!;
    [JsonIgnore]
    public virtual Product? Products { get; set; } = null!;
}
