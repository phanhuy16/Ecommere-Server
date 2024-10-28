

namespace Server.Entities;


public class Order
{
    public Guid Order_Id { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public int Total { get; set; }

    public virtual ICollection<SubProduct>? SubProducts { get; set; } = new HashSet<SubProduct>();

    public Order()
    {
        SubProducts = new HashSet<SubProduct>();
    }
}