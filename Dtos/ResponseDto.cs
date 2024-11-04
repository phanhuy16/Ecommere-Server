using System.Net;
using Server.Entities;

namespace Server.Dtos;

public class ResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = null!;
    public HttpStatusCode HttpStatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}