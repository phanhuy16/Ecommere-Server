using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Services;
public class ClaimsSetupService : IClaimsSetup
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly EFDataContext _context;
    private readonly ILogger<ClaimsSetupService> _logger;
    public ClaimsSetupService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, EFDataContext context, ILogger<ClaimsSetupService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Response<IList<Claim>>> GetAllClaims(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exits.");

            return new Response<IList<Claim>>()
            {
                IsSuccess = false,
                Message = "User does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        return new Response<IList<Claim>>()
        {
            Data = userClaims,
            IsSuccess = true,
            Message = "Succeeded",
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<Response<object>> AddClaimsToUser(string email, string claimName, string claimValue)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exits.");

            return new Response<object>()
            {
                IsSuccess = false,
                Message = "User does not exits",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var userClaim = new Claim(claimName, claimValue);

        var result = await _userManager.AddClaimAsync(user, userClaim);

        if (result.Succeeded)
        {
            return new Response<object>()
            {
                IsSuccess = true,
                Message = $"User {user.Email} has a claim {claimName} added to them",
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        return new Response<object>()
        {
            IsSuccess = false,
            Message = $"Unable to add claim {claimName} to the user {user.Email}",
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }
}
