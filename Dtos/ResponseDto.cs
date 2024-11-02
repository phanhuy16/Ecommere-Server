using System.Net;
using Server.Entities;

namespace Server.Dtos;

public class ResponseDto<T>
{
    public ResponseDto()
    {
    }
    public ResponseDto(T data)
    {
        Data = data;
        Token = Token;
        IsSuccess = true;
        Message = string.Empty;
    }
    public string Id { get; set; }
    public T Data { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public string? Token { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}