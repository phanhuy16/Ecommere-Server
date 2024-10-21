

namespace Server.Entities;

public class Report
{
    public Guid Report_Id { get; set; }
    public Guid Inventory_Id { get; set; }
    public Guid Product_Id { get; set; }
    //Foreign Key to User
    public string User_Id { get; set; } = string.Empty;
    public Guid QuantityOnHand { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
}