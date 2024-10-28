
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
public class OrdersController : ControllerBase
{
    // private readonly IOrder _orderService;
    // public OrdersController(IOrder orderService)
    // {
    //     _orderService = orderService;
    // }

    // [HttpGet]
    // [HttpGet, Route("getall")]
    // [ProducesResponseType(typeof(List<Order>), (int)HttpStatusCode.OK)]
    // public async Task<ActionResult> GetAll()
    // {
    //     var response = await _orderService.GetAll();
    //     return Ok(response);
    // }

    // [HttpGet, Route("getbyid/{OrderId}")]
    // public async Task<ActionResult> GetById(Guid OrderId)
    // {
    //     var response = await _orderService.GetById(OrderId);
    //     return Ok(response);
    // }

    // [HttpPost]
    // [Route("post")]
    // [ProducesResponseType(typeof(Response<Order>), (int)HttpStatusCode.OK)]
    // public async Task<ActionResult> Post([FromBody] Order or)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(new Response<Order>
    //         {
    //             IsSuccess = false,
    //             Message = "Invalid data."
    //         });
    //     }

    //     var response = await _orderService.Add(or);
    //     if (response.IsSuccess)
    //     {
    //         return Ok(response);
    //     }
    //     return BadRequest(response);
    // }

    // [HttpDelete]
    // [Route("delete/{OrderId}")]
    // [ProducesResponseType(typeof(Response<Order>), (int)HttpStatusCode.OK)]
    // public async Task<ActionResult> Delete(Guid OrderId)
    // {
    //     var response = await _orderService.Delete(OrderId);
    //     if (response.IsSuccess)
    //     {
    //         return Ok(response);
    //     }
    //     return BadRequest(response);
    // }

    // [HttpPut, Route("update/{id}")]
    // [ProducesResponseType(typeof(List<Response<Order>>), (int)HttpStatusCode.OK)]
    // [ProducesResponseType(typeof(Response<Order>), (int)HttpStatusCode.BadRequest)]
    // public async Task<ActionResult> Update([FromBody] Order or, Guid id)
    // {
    //     var response = await _orderService.Update(or, id);
    //     if (response.IsSuccess)
    //     {
    //         return Ok(response);
    //     }
    //     return BadRequest(response);
    // }
}