
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Dtos;

namespace Server.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRole _roleService;
    public RolesController(IRole roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleResponseDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        var response = await _roleService.CreateRole(createRoleDto);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(RoleResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetRoles()
    {
        var response = await _roleService.GetRoles();
        return response;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RoleResponseDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var response = await _roleService.DeleteRole(id);

        return Ok(response);
    }

    [HttpPost("assign")]
    [ProducesResponseType(typeof(RoleResponseDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AssignRole([FromBody] RoleAssignDto roleAssignDto)
    {

        var response = await _roleService.AssignRole(roleAssignDto);

        return Ok(response);
    }
}