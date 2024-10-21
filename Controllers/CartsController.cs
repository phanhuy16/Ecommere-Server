
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICart _cartService;
    public CartsController(ICart cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [HttpGet, Route("getall")]
    [ProducesResponseType(typeof(List<Cart>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var response = await _cartService.GetAll();
        return Ok(response);
    }

    [HttpGet, Route("getbyid/{CartId}")]
    public async Task<ActionResult> GetById(Guid CartId)
    {
        var response = await _cartService.GetById(CartId);
        return Ok(response);
    }

    [HttpPost]
    [Route("post")]
    [ProducesResponseType(typeof(Response<Cart>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Post([FromBody] Cart cart)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<Cart>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _cartService.Add(cart);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete]
    [Route("delete/{CartId}")]
    [ProducesResponseType(typeof(Response<Cart>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Delete(Guid CartId)
    {
        var response = await _cartService.Delete(CartId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut, Route("update/{id}")]
    [ProducesResponseType(typeof(List<Response<Cart>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Cart>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update([FromBody] Cart cart, Guid id)
    {
        var response = await _cartService.Update(cart, id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}