

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
    public Guid Product_Id { get; set; }
    public Guid? Order_Id { get; set; }
    public virtual Order? Order { get; set; } = null!;
    public virtual Product? Product { get; set; } = null!;
}