

namespace Server.Dtos.Requests
{
    public class TokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}