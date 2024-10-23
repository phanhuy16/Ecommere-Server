using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Server.Contracts;
using Server.Entities;
using Server.Dtos;

namespace Server.Services;

public class AccountService : IAccount
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        // Kiểm tra xem người dùng đã tồn tại chưa
        var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUserByEmail != null)
        {
            return new BadRequestObjectResult(new ResponseDto
            {
                IsSuccess = false,
                Message = "User with this email already exists."
            });
        }

        var existingUserByUserName = await _userManager.FindByNameAsync(registerDto.Email);
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
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            UserName = registerDto.Email,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new BadRequestObjectResult($"Registration failed: {errors}");
        }

        if (registerDto.Roles == null)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            foreach (var role in registerDto.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        var token = GenerateToken(user);

        return new OkObjectResult(new ResponseDto
        {
            Token = token,
            IsSuccess = true,
            Message = "Register Successfully.",
        });
    }

    public async Task<ActionResult<ResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found with this email",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid Password.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var token = GenerateToken(user);

        return new ResponseDto
        {
            Token = token,
            Id = user.Id,
            IsSuccess = true,
            Message = "Login Success.",
            HttpStatusCode = HttpStatusCode.OK,
        };
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetSection("SecurityKey").Value!);

        var roles = _userManager.GetRolesAsync(user).Result;

        List<Claim> claims = [
            new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new (JwtRegisteredClaimNames.Name, user.FullName ?? ""),
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

    public async Task<ActionResult<UserDetailDto>> GetUserDetail()
    {
        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(currentUserId);

        if (user is null)
        {
            return new NotFoundObjectResult(new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found"
            });
        }

        var userDetailDto = new UserDetailDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = [.. await _userManager.GetRolesAsync(user)],
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            AccessFailedCount = user.AccessFailedCount,
        };

        return new OkObjectResult(userDetailDto);
    }

    public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers()
    {
        var users = await _userManager.Users.Select(u => new UserDetailDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            Roles = _userManager.GetRolesAsync(u).Result.ToArray()
        }).ToListAsync();

        return new OkObjectResult(users);
    }
}