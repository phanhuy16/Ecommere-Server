

using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Dtos;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Services;


public class SetupService : ISetUp
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly EFDataContext _context;
    private readonly ILogger<SetupService> _logger;

    public SetupService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, EFDataContext context, ILogger<SetupService> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Response<List<IdentityRole>>> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return new Response<List<IdentityRole>>
        {
            Data = roles,
            Message = "Role success",
            IsSuccess = true,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<Response<string>> CreateRole(string name)
    {

        var roleExits = await _roleManager.RoleExistsAsync(name);

        if (!roleExits)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation($"The Role {name} has been added successfully.");

                return new Response<string>()
                {
                    Data = $"The role {name} has been added successfully.",
                    IsSuccess = true,
                    Message = "Successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                _logger.LogInformation($"The Role {name} has not been added.");

                return new Response<string>()
                {
                    Data = $"The role {name} has not been added.",
                    IsSuccess = false,
                    Message = "Failed",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }
        }


        return new Response<string>()
        {
            IsSuccess = false,
            Message = "Errors",
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }

    public async Task<Response<List<User>>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        return new Response<List<User>>
        {
            Data = users,
            Message = "Role success",
            IsSuccess = true,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<Response<string>> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exits.");

            return new Response<string>()
            {
                IsSuccess = false,
                Message = "User does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var roleExits = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExits)
        {
            _logger.LogInformation($"The role {email} does not exits.");

            return new Response<string>()
            {
                IsSuccess = false,
                Message = "Role does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            _logger.LogInformation($"The user was not abel to be added to the role.");

            return new Response<string>()
            {
                IsSuccess = true,
                Message = "Success, user has been addad to the role.",
                HttpStatusCode = HttpStatusCode.OK
            };
        }
        else
        {
            return new Response<string>()
            {
                IsSuccess = false,
                Message = "The user was not abel to be added to the role.",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    public async Task<Response<IList<string>>> GetUserRoles(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exits.");

            return new Response<IList<string>>()
            {
                IsSuccess = false,
                Message = "User does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new Response<IList<string>>()
        {
            Data = roles,
            IsSuccess = true,
            Message = "Success",
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<Response<string>> RemoveUserFromRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exits.");

            return new Response<string>()
            {
                IsSuccess = false,
                Message = "User does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var roleExits = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExits)
        {
            _logger.LogInformation($"The role {email} does not exits.");

            return new Response<string>()
            {
                IsSuccess = false,
                Message = "Role does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            return new Response<string>()
            {
                IsSuccess = true,
                Message = $"User {email} has been removed from role {roleName}",
                HttpStatusCode = HttpStatusCode.OK
            };
        }
        return new Response<string>()
        {
            IsSuccess = false,
            Message = $"Unable to remove User {email} from role {roleName}",
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }
}