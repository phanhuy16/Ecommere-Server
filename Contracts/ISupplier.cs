using Microsoft.AspNetCore.Mvc;
using Server.Entities;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface ISupplier
{
    Task<Response<List<Supplier>>> GetAllSupplier();
    Task<IActionResult> GetPaginationSupplier(PaginationFilter filter, string route);
    Task<Response<Supplier>> GetById(Guid SupplierId);
    Task<Response<Supplier>> Post(Supplier sup);
    Task<Response<Supplier>> Update(Supplier sup, Guid SupplierId);
    Task<Response<Supplier>> Delete(Guid SupplierId);
    Task<Response<FileContentResult>> ExportExcel();
}