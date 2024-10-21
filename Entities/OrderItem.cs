

namespace Server.Entities;

public class OrderItem
{
    public Guid Order_Id { get; set; }
    public Guid Product_Id { get; set; }
    public Guid Inventory_Id { get; set; }
    public Guid Report_Id { get; set; }
    public byte Ordered_Quantity { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Report Report { get; set; } = null!;
}