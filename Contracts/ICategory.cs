

using Microsoft.AspNetCore.Mvc;
using Server.Entities;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface ICategory
{
    Task<Response<List<Category>>> GetAllCategories();
    Task<IActionResult> GetPaginationCategories(PaginationFilter filter, string route);
    Task<Response<Category>> GetById(Guid CategoryId);
    Task<Response<Category>> Add(Category cate);
    Task<Response<Category>> Update(Category cate, Guid CategoryId);

    Task<Response<Category>> Delete(Guid CategoryId);
}