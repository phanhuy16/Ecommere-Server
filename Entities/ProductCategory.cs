

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Entities;


public class ProductCategory
{
    [Key]
    public Guid? ProductId { get; set; }

    [JsonIgnore]
    public Product Product { get; set; } = null!;
    [Key]
    public Guid? CategoryId { get; set; }

    [JsonIgnore]
    public Category Category { get; set; } = null!;
}