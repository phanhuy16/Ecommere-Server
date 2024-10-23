
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;

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
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RegisterCustomer(Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid data."
                });
            }

            var response = await _customerService.RegisterCustomer(customers);

            return Ok(response);
        }
    }
}