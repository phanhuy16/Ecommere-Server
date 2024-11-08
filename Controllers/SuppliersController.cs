
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Controllers;


[Authorize(Roles = "Admin")]

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplier _supplierService;
    public SuppliersController(ISupplier supplierService)
    {
        _supplierService = supplierService;
    }


    // [Authorize(Roles = "User")]

    [HttpGet, Route("get-all")]
    [ProducesResponseType(typeof(List<Supplier>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllSupplier()
    {
        var response = await _supplierService.GetAllSupplier();
        return Ok(response);
    }

    [HttpGet, Route("get-pagination")]
    [ProducesResponseType(typeof(List<Supplier>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetPaginationSupplier([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var response = await _supplierService.GetPaginationSupplier(filter, route);

        return Ok(response);

    }

    [HttpGet, Route("get-by-id")]
    public async Task<ActionResult> GetById([FromQuery] Guid id)
    {
        var response = await _supplierService.GetById(id);
        return Ok(response);
    }


    [HttpGet, Route("export-excel")]
    public async Task<ActionResult> ExportExcel()
    {
        var response = await _supplierService.ExportExcel();
        return Ok(response);
    }

    [HttpPost, Route("add-new")]
    [ProducesResponseType(typeof(List<Response<Supplier>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Supplier>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Post([FromBody] Supplier supplier)
    {
        var response = await _supplierService.Post(supplier);

        return Ok(response);
    }

    [HttpPut, Route("update")]
    [ProducesResponseType(typeof(List<Response<Supplier>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Supplier>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update([FromBody] Supplier supplier, [FromQuery] Guid id)
    {
        var response = await _supplierService.Update(supplier, id);
        return Ok(response);
    }

    [HttpDelete, Route("delete")]
    [ProducesResponseType(typeof(Response<Supplier>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Supplier>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        var response = await _supplierService.Delete(id);
        return Ok(response);
    }
}