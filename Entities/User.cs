using Microsoft.AspNetCore.Identity;

namespace Server.Entities;

public class User : IdentityUser
{
    public string? FullName { get; set; } = string.Empty;
    public string? FisrtName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
}
