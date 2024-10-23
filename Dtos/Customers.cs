

namespace Server.Dtos
{
    public class Customers
    {
        public string? FisrtName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string>? Roles { get; set; } = null;
    }
}