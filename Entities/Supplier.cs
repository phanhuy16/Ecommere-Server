using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public float Price { get; set; }
    public string Contact { get; set; } = string.Empty;
    public bool? IsTalking { get; set; } = false;
    public string? Email { get; set; } = string.Empty;
    public int Active { get; set; }
    public string? Imgae { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid CategoryId { get; set; }
    [JsonIgnore]
    public Category Category { get; set; } = null!;
    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product Product { get; set; } = null!;
}