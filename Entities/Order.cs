

using System.Text.Json.Serialization;

namespace Server.Entities;


public class Order
{
    public Guid Id { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public int Total { get; set; }

    [JsonIgnore]
    public virtual ICollection<SubProduct>? SubProducts { get; set; } = null!;
}