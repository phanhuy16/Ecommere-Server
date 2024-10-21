using Microsoft.AspNetCore.Mvc;
using Server.Dtos;


namespace Server.Contracts;

public interface IRole
{
    Task<IActionResult> CreateRole(CreateRoleDto createRoleDto);

    Task<ActionResult<IEnumerable<RoleResponseDto>>> GetRoles();

    Task<IActionResult> DeleteRole(string id);

    Task<IActionResult> AssignRole(RoleAssignDto roleAssignDto);
}