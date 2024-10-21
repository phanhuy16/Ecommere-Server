

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Dtos;
using Server.Entities;

namespace Server.Services;


public class RoleService : IRole
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
    {
        if (string.IsNullOrEmpty(createRoleDto.RoleName))
        {
            return new BadRequestObjectResult("Role name is required");
        }

        var roleExits = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);

        if (roleExits)
        {
            return new BadRequestObjectResult("Role already exist");
        }

        var roleResult = await _roleManager.CreateAsync(new IdentityRole(createRoleDto.RoleName));

        if (roleResult.Succeeded)
        {
            return new OkObjectResult(new { message = "Role Created successfully" });
        }

        return new BadRequestObjectResult("Role creation failed.");
    }

    public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetRoles()
    {
        var roles = await _roleManager.Roles.Select(r => new RoleResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            TotalUsers = _userManager.GetUsersInRoleAsync(r.Name!).Result.Count
        }).ToListAsync();

        return new OkObjectResult(roles);
    }

    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
        {
            return new NotFoundObjectResult("Role not found.");
        }

        var result = await _roleManager.DeleteAsync(role);

        if (result.Succeeded)
        {
            return new OkObjectResult(new { message = "Role delete successfully." });
        }

        return new BadRequestObjectResult("Role deletion failed.");
    }

    public async Task<IActionResult> AssignRole(RoleAssignDto roleAssignDto)
    {
        var user = await _userManager.FindByIdAsync(roleAssignDto.UserId);

        if (user is null)
        {
            return new NotFoundObjectResult("User not found.");
        }

        var role = await _roleManager.FindByIdAsync(roleAssignDto.RoleId);

        if (user is null)
        {
            return new NotFoundObjectResult("Role not found.");
        }

        var result = await _userManager.AddToRoleAsync(user, role.Name!);

        if (result.Succeeded)
        {
            return new OkObjectResult(new { message = "Role assigned successfully." });
        }

        var error = result.Errors.FirstOrDefault();

        return new BadRequestObjectResult(error!.Description);
    }
}