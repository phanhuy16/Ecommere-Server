

using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities;
public class RefreshToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public bool IsUesd { get; set; }
    public bool IsRevorked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
