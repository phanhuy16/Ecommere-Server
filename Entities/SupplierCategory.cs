
using System.Text.Json.Serialization;

namespace Server.Entities
{
    public class SupplierCategory
    {
        public Guid SupplierId { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; } = null!;

        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; } = null!;
    }
}