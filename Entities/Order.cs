

namespace Server.Entities;


public class Order
{
    public Guid Order_Id { get; set; }
    public Guid? Cart_Id { get; set; }
    public DateTime Order_Date { get; set; }
    public string Order_Decs { get; set; } = string.Empty;
    public decimal Order_Fee { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public Cart Cart { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
}