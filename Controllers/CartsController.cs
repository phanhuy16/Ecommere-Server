
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

    [HttpGet, Route("get-cart")]
    public async Task<ActionResult> GetCatItem([FromQuery] string id)
    {
        var response = await _cartService.GetCatItem(id);
        return Ok(response);
    }

    [HttpPost]
    [Route("add-new")]
    [ProducesResponseType(typeof(Response<Cart>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> AddMultiple([FromBody] Cart cart)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<Cart>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _cartService.AddMultiple(cart);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(typeof(Response<Cart>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        var response = await _cartService.Delete(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }


}