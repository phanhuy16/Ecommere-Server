
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;
using Server.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomer _customerService;
        public CustomersController(ICustomer customerService)
        {
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sign-up")]
        [ProducesResponseType(typeof(ResponseDto<Customers>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RegisterCustomer(Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<Customers>
                {
                    IsSuccess = false,
                    Message = "Invalid data."
                });
            }

            var response = await _customerService.RegisterCustomer(customers);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(ResponseDto<LoginDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<LoginDto>), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<ResponseDto<LoginDto>>> Login(LoginDto login)
        {
            var response = await _customerService.Login(login);

            return Ok(response);

        }

        // [AllowAnonymous]
        // [HttpPost("send")]
        // [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        // public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        // {
        //     try
        //     {
        //         await _customerService.SendEmailAsync(request);
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         throw;
        //     }

        // }
        // [AllowAnonymous]
        // [HttpPost]
        // [Route("verification")]
        // [Consumes("application/json")]
        // [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        // public async Task<ActionResult> EmailVerification([FromQuery] string? id, [FromBody] string? code)
        // {

        //     var response = await _customerService.EmailVerification(id, code);

        //     return Ok(response);
        // }
    }
}