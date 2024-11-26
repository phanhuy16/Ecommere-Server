
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Filter;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Controllers;

[Authorize(Roles = "Admin")]

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProduct _productService;
    public ProductsController(IProduct productService)
    {
        _productService = productService;
    }


    // [Authorize(Roles = "User")]

    [HttpGet, Route("get-all-values")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllSubProducts()
    {
        var response = await _productService.GetAllSubProducts();

        return Ok(response);
    }

    [HttpGet, Route("get-all")]
    [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllProducts()
    {
        var response = await _productService.GetAllProducts();

        return Ok(response);
    }

    [HttpGet, Route("get-best-seller")]
    [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetBestSellerProducts()
    {
        var response = await _productService.GetBestSellerProducts();

        return Ok(response);
    }

    [HttpGet, Route("get-pagination")]
    [ProducesResponseType(typeof(List<IEnumerable<Product>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetPaginationProducts([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var response = await _productService.GetPaginationProducts(filter, route);

        return Ok(response);
    }

    [HttpGet, Route("get-by-id")]
    public async Task<ActionResult> GetById([FromQuery] Guid id)
    {
        var response = await _productService.GetById(id);
        return Ok(response);
    }

    [HttpGet, Route("search")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string slug)
    {
        var response = await _productService.Search(slug);
        return Ok(response);
    }

    [HttpPost, Route("filter-product")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> FilterProducts([FromBody] FilterDto filter)
    {
        var response = await _productService.FilterProductsAsync(filter);
        return Ok(response);
    }


    [HttpPost, Route("add-new")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> AddProduct([FromBody] Product product)
    {
        var response = await _productService.AddProduct(product);
        return Ok(response);
    }

    [HttpPost, Route("add-sub-product")]
    [ProducesResponseType(typeof(List<Response<SubProduct>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<SubProduct>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> AddSubProduct([FromBody] SubProduct subProduct)
    {
        var response = await _productService.AddSubProduct(subProduct);
        return Ok(response);
    }

    [HttpPut, Route("update")]
    [ProducesResponseType(typeof(List<Response<Product>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update([FromBody] Product product, [FromQuery] Guid id)
    {
        var response = await _productService.Update(product, id);
        return Ok(response);
    }

    [HttpPut, Route("update-sub-product")]
    [ProducesResponseType(typeof(List<Response<SubProduct>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<SubProduct>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> UpdateSubProduct([FromBody] SubProduct sub, [FromQuery] Guid id)
    {
        var response = await _productService.UpdateSubProduct(sub, id);
        return Ok(response);
    }

    [HttpDelete, Route("delete")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete([FromQuery] Guid ProductId)
    {
        var response = await _productService.Delete(ProductId);
        return Ok(response);
    }

    [HttpDelete, Route("delete-sub-product")]
    [ProducesResponseType(typeof(Response<SubProduct>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<SubProduct>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> DeleteSubProduct([FromQuery] Guid id)
    {
        var response = await _productService.DeleteSubProduct(id);
        return Ok(response);
    }
}