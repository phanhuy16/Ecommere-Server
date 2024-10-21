using Microsoft.AspNetCore.Identity;

namespace Server.Entities;

public class User : IdentityUser
{
    public string? FullName { get; set; } = string.Empty;
    public Cart Cart { get; set; } = null!;
    public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
