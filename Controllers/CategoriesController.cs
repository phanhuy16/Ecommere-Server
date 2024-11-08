
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
public class CategoriesController : ControllerBase
{
    private readonly ICategory _categoryService;
    public CategoriesController(ICategory categoryService)
    {
        _categoryService = categoryService;
    }

    // [Authorize(Roles = "User")]

    [HttpGet, Route("get-all")]
    [ProducesResponseType(typeof(List<Category>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllCategories()
    {
        var response = await _categoryService.GetAllCategories();
        return Ok(response);
    }

    [HttpGet, Route("get-pagination")]
    [ProducesResponseType(typeof(List<Category>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetPaginationCategories([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;

        var response = await _categoryService.GetPaginationCategories(filter, route);

        return Ok(response);
    }

    [HttpGet, Route("get-by-id")]
    public async Task<ActionResult> GetById([FromQuery] Guid id)
    {
        var response = await _categoryService.GetById(id);
        return Ok(response);
    }

    [HttpPost]
    [Route("add-new")]
    [ProducesResponseType(typeof(Response<Category>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Post([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<Category>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _categoryService.Add(category);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(typeof(Response<Category>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        var response = await _categoryService.Delete(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut, Route("update")]
    [ProducesResponseType(typeof(List<Response<Category>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Category>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update([FromQuery] Category category, Guid id)
    {
        var response = await _categoryService.Update(category, id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}