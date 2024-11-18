
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Controllers;

// [Authorize(Roles = "Admin")]

[ApiController]
[Route("api/[controller]")]
public class SetupsController : ControllerBase
{
    private readonly ISetUp _setUpService;
    public SetupsController(ISetUp setUpService)
    {
        _setUpService = setUpService;
    }

    [HttpGet]
    [Route("get-all-role")]
    [ProducesResponseType(typeof(Response<List<IdentityRole>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<List<IdentityRole>>>> GetAllRoles()
    {
        var response = await _setUpService.GetAllRoles();
        return Ok(response);
    }

    [HttpGet]
    [Route("get-all-user")]
    [ProducesResponseType(typeof(Response<List<User>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<List<User>>>> GetAllUsers()
    {
        var response = await _setUpService.GetAllUsers();
        return Ok(response);
    }

    [HttpGet]
    [Route("get-user-roles")]
    [ProducesResponseType(typeof(Response<IList<string>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<IList<string>>>> GetUserRoles(string email)
    {
        var response = await _setUpService.GetUserRoles(email);
        return Ok(response);
    }

    [HttpPost]
    [Route("add-role")]
    [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateRole([FromBody] string name)
    {
        var response = await _setUpService.CreateRole(name);

        return Ok(response);
    }

    [HttpPost]
    [Route("add-user-to-role")]
    [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddUserToRole([FromQuery] string email, string name)
    {
        var response = await _setUpService.AddUserToRole(email, name);

        return Ok(response);
    }

    [HttpDelete]
    [Route("remove-user-from-role")]
    [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteRole(string email, string name)
    {
        var response = await _setUpService.RemoveUserFromRole(email, name);

        return Ok(response);
    }
}