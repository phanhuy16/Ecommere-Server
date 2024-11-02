

using Server.Utilities.Response;

namespace Server.Utilities.Pagination;

public class PagedResponse<T> : Response<T>
{
    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber;
        PageSize = pageSize;
        this.Data = data;
        this.Message = null;
        this.IsSuccess = true;
    }
    public int PageNumber { get; }
    public int PageSize { get; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }
}