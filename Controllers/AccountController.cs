
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;

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
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _accountService.Register(registerDto);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<ResponseDto>> Login(LoginDto loginDto)
    {
        var response = await _accountService.Login(loginDto);

        return Ok(response);

    }

    [HttpGet]
    [HttpGet, Route("detail")]
    [ProducesResponseType(typeof(UserDetailDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserDetailDto>> GetUserDetail()
    {
        var response = await _accountService.GetUserDetail();
        return response;
    }

    [HttpGet, Route("get")]
    [ProducesResponseType(typeof(UserDetailDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers()
    {
        var response = await _accountService.GetUsers();
        return response;
    }
}