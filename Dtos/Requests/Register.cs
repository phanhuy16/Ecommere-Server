using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Requests;

public class Register
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public List<string>? Roles { get; set; } = null;
}
