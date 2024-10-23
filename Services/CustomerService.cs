

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Contracts;
using Server.Dtos;
using Server.Entities;

namespace Server.Services;

public class CustomerService : ICustomer
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ActionResult> RegisterCustomer(Customers customer)
    {
        // Kiểm tra xem người dùng đã tồn tại chưa
        var existingUserByEmail = await _userManager.FindByEmailAsync(customer.Email);
        if (existingUserByEmail != null)
        {
            return new BadRequestObjectResult(new ResponseDto
            {
                IsSuccess = false,
                Message = "User with this email already exists."
            });
        }

        var existingUserByUserName = await _userManager.FindByNameAsync(customer.Email);
        if (existingUserByUserName != null)
        {
            return new BadRequestObjectResult(new ResponseDto
            {
                IsSuccess = false,
                Message = "User with this user already exists."
            });
        }

        var user = new User
        {
            Email = customer.Email,
            UserName = customer.Email,
            LastName = customer.LastName,
            FisrtName = customer.FisrtName,
        };

        var result = await _userManager.CreateAsync(user, customer.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new BadRequestObjectResult($"Registration failed: {errors}");
        }

        if (customer.Roles == null)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            foreach (var role in customer.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        var token = GenerateToken(user);

        return new OkObjectResult(new ResponseDto
        {
            Token = token,
            Id = user.Id,
            IsSuccess = true,
            Message = "Register Successfully.",
        });
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetSection("SecurityKey").Value!);

        var roles = _userManager.GetRolesAsync(user).Result;

        List<Claim> claims = [
            new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new (JwtRegisteredClaimNames.Name, user.FisrtName ?? ""),
                new (JwtRegisteredClaimNames.Name, user.LastName ?? ""),
                new (JwtRegisteredClaimNames.NameId,user.Id ?? ""),
                new(JwtRegisteredClaimNames.Aud, _configuration.GetSection("Jwt").GetSection("Audience").Value!), new (JwtRegisteredClaimNames.Iss, _configuration.GetSection("Jwt").GetSection("Issuer").Value!)
        ];

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
