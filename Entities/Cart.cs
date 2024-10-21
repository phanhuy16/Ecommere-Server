namespace Server.Entities;

public class Cart
{
    public Guid Cart_Id { get; set; }
    public string User_Id { get; set; } = string.Empty;
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public User User { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
