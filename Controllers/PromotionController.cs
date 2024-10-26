
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PromotionController : ControllerBase
{
    private readonly IPromotion _promotionService;
    public PromotionController(IPromotion promotionService)
    {
        _promotionService = promotionService;
    }

    // [Authorize]
    [HttpPost, Route("add-new")]
    [ProducesResponseType(typeof(Response<Promotion>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Promotion>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> AddPromotion([FromBody] Promotion promotion)
    {
        var response = await _promotionService.AddPromotion(promotion);
        return Ok(response);
    }
}
