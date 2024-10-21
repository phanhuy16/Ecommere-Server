
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItem _orderItemService;
    public OrderItemsController(IOrderItem orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    [HttpGet, Route("getall")]
    [ProducesResponseType(typeof(List<OrderItem>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var response = await _orderItemService.GetAll();
        return Ok(response);
    }

    [HttpGet, Route("getbyid/{OrderItemId}")]
    public async Task<ActionResult> GetById(Guid OrderItemId)
    {
        var response = await _orderItemService.GetById(OrderItemId);
        return Ok(response);
    }
}