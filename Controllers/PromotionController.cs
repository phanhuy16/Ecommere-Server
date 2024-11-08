
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Controllers;


[Authorize(Roles = "Admin")]

[ApiController]
[Route("api/[controller]")]
public class PromotionController : ControllerBase
{
    private readonly IPromotion _promotionService;
    public PromotionController(IPromotion promotionService)
    {
        _promotionService = promotionService;
    }


    // [Authorize(Roles = "User")]

    [HttpGet, Route("get-all")]
    [ProducesResponseType(typeof(List<Promotion>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllPromotion()
    {
        var response = await _promotionService.GetAllPromotion();

        return Ok(response);
    }

    [HttpPost, Route("add-new")]
    [ProducesResponseType(typeof(Response<Promotion>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Promotion>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> AddPromotion([FromBody] Promotion promotion)
    {
        var response = await _promotionService.AddPromotion(promotion);
        return Ok(response);
    }


    [HttpPut, Route("update")]
    [ProducesResponseType(typeof(List<Response<Promotion>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Promotion>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> UpdatePromotion([FromBody] Promotion promotion, [FromQuery] Guid id)
    {
        var response = await _promotionService.UpdatePromotion(promotion, id);
        return Ok(response);
    }

    [HttpDelete, Route("delete")]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Product>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> DeletePromotion([FromQuery] Guid id)
    {
        var response = await _promotionService.DeletePromotion(id);
        return Ok(response);
    }
}
