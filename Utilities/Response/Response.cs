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
        Errors = null;
        Message = string.Empty;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string[] Errors { get; set; }
    public T Data { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }

    // ID
    public Guid SupplierId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ProductId { get; set; }
    public Guid SubProductId { get; set; }
    public Guid CartId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ReportId { get; set; }
}