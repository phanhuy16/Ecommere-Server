using System.Net;

namespace Server.Dtos;

public class ResponseDto
{
    public string? Token { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}