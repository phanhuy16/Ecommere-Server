
using Microsoft.AspNetCore.Mvc;
using Server.Entities;
using Server.Utilities.Filter;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface IProduct
{
    Task<List<Product>> GetAllProducts();
    Task<IActionResult> GetPaginationProducts(PaginationFilter filter, string route);
    Task<Product> GetById(Guid ProductId);
    Task<Response<Product>> AddProduct(Product pro);
    Task<Response<SubProduct>> AddSubProduct(SubProduct subPro);
    Task<Response<Product>> Update(Product pro, Guid ProductId);
    Task<Response<Product>> Delete(Guid ProductId);
    Task<IEnumerable<Product>> Search(string slug);
    Task<Response<FilterValues>> GetAllSubProducts();
    Task<List<Product>> FilterProductsAsync(FilterDto filter);
    Task<Response<SubProduct>> DeleteSubProduct(Guid SubProductId);
    Task<Response<SubProduct>> UpdateSubProduct(SubProduct sub, Guid SubProductId);
}