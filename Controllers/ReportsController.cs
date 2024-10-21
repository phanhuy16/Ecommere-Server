
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReport _reportService;
    public ReportsController(IReport reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    [HttpGet, Route("getall")]
    [ProducesResponseType(typeof(List<Report>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var response = await _reportService.GetAll();
        return Ok(response);
    }

    [HttpGet, Route("getbyid/{ReportId}")]
    public async Task<ActionResult> GetById(Guid ReportId)
    {
        var response = await _reportService.GetById(ReportId);
        return Ok(response);
    }

    [HttpPost]
    [Route("post")]
    [ProducesResponseType(typeof(Response<Report>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Post([FromBody] Report rep)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<Report>
            {
                IsSuccess = false,
                Message = "Invalid data."
            });
        }

        var response = await _reportService.Add(rep);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete]
    [Route("delete/{ReportId}")]
    [ProducesResponseType(typeof(Response<Report>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Delete(Guid ReportId)
    {
        var response = await _reportService.Delete(ReportId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut, Route("update/{id}")]
    [ProducesResponseType(typeof(List<Response<Report>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Response<Report>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update([FromBody] Report rep, Guid id)
    {
        var response = await _reportService.Update(rep, id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}