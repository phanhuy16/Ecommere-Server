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

    [JsonIgnore]
    [DisplayName("Categories")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public virtual List<Category> Categories { get; set; } = new List<Category>();
}