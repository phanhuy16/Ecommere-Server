namespace Server.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Qty { get; set; }

    public Guid SubProductId { get; set; }
    public Guid ProductId { get; set; }
    public virtual SubProduct? SubProduct { get; set; } = null!;
    public virtual Product? Products { get; set; } = null!;
}
