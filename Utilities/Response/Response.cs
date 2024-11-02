using System.Net;

namespace Server.Utilities.Response;

public class Response<T>
{
    public Response()
    {
    }

    public Response(T data)
    {
        Data = data;
        IsSuccess = true;
        Message = string.Empty;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }

    // ID
    public Guid Id { get; set; }
}