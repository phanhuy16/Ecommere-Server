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
using Server.Data;
using Server.Dtos.Requests;
using Server.Utilities.Response;

namespace Server.Services;

public class AccountService : IAccount
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EFDataContext _context;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly ILogger<AccountService> _logger;
    public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, EFDataContext context, TokenValidationParameters tokenValidationParameters, ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _tokenValidationParameters = tokenValidationParameters;
        _logger = logger;
    }

    public async Task<Response<object>> Register(Register register)
    {
        // Kiểm tra xem người dùng đã tồn tại chưa
        var existingEmail = await _userManager.FindByEmailAsync(register.Email);
        if (existingEmail != null)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = "User with this email already exists.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var existingUserName = await _userManager.FindByNameAsync(register.Email);
        if (existingUserName != null)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = "User with this user already exists.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var user = new User
        {
            Email = register.Email,
            UserName = register.UserName,
        };

        var result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Response<object>
            {
                IsSuccess = false,
                Message = $"Registration failed: {errors}.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        // We need to add the user to a role
        if (register.Roles == null)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            foreach (var role in register.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        var token = await GenerateToken(user);

        // return token;
        return new Response<object>
        {
            Message = "Registration successful.",
            Data = token,
            HttpStatusCode = HttpStatusCode.OK,
            IsSuccess = true,
        };
    }

    public async Task<Response<object>> Login(Login login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);

        if (user == null)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = "User not found with this email",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!result)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = "Invalid Password.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var token = await GenerateToken(user);

        return new Response<object>
        {
            Message = "Login successful.",
            Data = token,
            HttpStatusCode = HttpStatusCode.OK,
            IsSuccess = true,
        };
    }

    public async Task<ResponseDTO> RefreshToken(TokenRequest tokenRequest)
    {

        var result = await VerifyAndGenerateToken(tokenRequest);

        if (result == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid token",
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        return result;
    }

    private async Task<ResponseDTO> GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetSection("SecurityKey").Value!);

        var claims = await GetAllValdClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwtToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsUesd = false,
            IsRevorked = false,
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            Token = RandomString(35) + Guid.NewGuid(),
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return new ResponseDTO()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            IsSuccess = true,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    // Get all valid claims for the corresponding user
    private async Task<List<Claim>> GetAllValdClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new (JwtRegisteredClaimNames.NameId,user.Id),
            new (JwtRegisteredClaimNames.Sub,user.Email ?? ""),
            new(JwtRegisteredClaimNames.Aud, _configuration.GetSection("Jwt").GetSection("Audience").Value!), new (JwtRegisteredClaimNames.Iss, _configuration.GetSection("Jwt").GetSection("Issuer").Value!),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Getting the claims that we have assigned to the user
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        // Get the user role and add it to the claims
        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(userRole);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }
        }

        return claims;
    }

    private async Task<ResponseDTO> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            // Validation 1 - validation jwt token format
            var tokenInVerification = tokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

            // Validation 2 - validate encryption alg
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return null!;
                }
            }

            // Validation 3 - validate expiry date
            var expClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim == null || string.IsNullOrEmpty(expClaim.Value))
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token does not contain an expiry date",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var utcExpiryDate = long.Parse(expClaim.Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate < DateTime.UtcNow)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token has not yet expired",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Validation 4 - validate existence of the token 
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token do not exist",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Validation 5 - validate if used
            if (storedToken.IsUesd)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token has been used",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Validation 6 - validate if revoked
            if (storedToken.IsRevorked)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token has been revoked",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }


            // Validation 7 - validate the id
            var jtiClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
            if (jtiClaim == null || string.IsNullOrEmpty(jtiClaim.Value))
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token does not contain a JTI",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var jti = jtiClaim.Value;

            if (storedToken.JwtId != jti)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = "Token doesn't match",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Update current token

            storedToken.IsUesd = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            // Generate a new token
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            return await GenerateToken(dbUser);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTimeVal;
    }

    private string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
}