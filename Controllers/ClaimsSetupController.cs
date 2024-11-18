
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Utilities.Response;

namespace Server.Controllers
{

    // [Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsSetupController : ControllerBase
    {
        private readonly IClaimsSetup _claimsSetUpService;
        public ClaimsSetupController(IClaimsSetup claimsSetUpService)
        {
            _claimsSetUpService = claimsSetUpService;
        }
        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(Response<List<Claim>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Response<List<Claim>>>> GetAllClaims(string email)
        {
            var response = await _claimsSetUpService.GetAllClaims(email);
            return Ok(response);
        }

        [HttpPost]
        [Route("add-claim-to-user")]
        [ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddClaimsToUser(string email, string name, string value)
        {
            var response = await _claimsSetUpService.AddClaimsToUser(email, name, value);

            return Ok(response);
        }
    }
}