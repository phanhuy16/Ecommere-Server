
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;
using Server.Dtos.Requests;

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
    [ProducesResponseType(typeof(ResponseDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Register(Register register)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDTO
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
    [ProducesResponseType(typeof(ResponseDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDTO), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<ResponseDTO>> Login(Login login)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDTO
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
    [Route("refreshtoken")]
    [ProducesResponseType(typeof(ResponseDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDTO), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<ResponseDTO>> RefreshToken(TokenRequest tokenRequest)
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