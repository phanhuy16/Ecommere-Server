
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;
using Server.Dtos.Requests;
using Server.Utilities.Response;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccount _accountService;

    public AccountController(IAccount accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Register(Register register)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _accountService.Register(register);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> Login(Login login)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _accountService.Login(login);

        return Ok(response);

    }

    [AllowAnonymous]
    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> RefreshToken(TokenRequest tokenRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _accountService.RefreshToken(tokenRequest);

        return Ok(response);

    }
}