

namespace Server.Utilities.Pagination;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}