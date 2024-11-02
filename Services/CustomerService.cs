

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Server.Contracts;
using Server.Dtos;
using Server.Entities;
using Server.Helper;

namespace Server.Services;

public class CustomerService : ICustomer
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly MailSettings _mailSettings;

    public CustomerService(UserManager<User> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOptions<MailSettings> mailSettings)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _mailSettings = mailSettings.Value;

    }


    public async Task<ResponseDto<Customers>> RegisterCustomer(Customers customer)
    {
        // Kiểm tra xem người dùng đã tồn tại chưa
        var existingUserByEmail = await _userManager.FindByEmailAsync(customer.Email);
        if (existingUserByEmail != null)
        {
            return new ResponseDto<Customers>
            {
                IsSuccess = false,
                Message = "User with this email already exists."
            };
        }

        var existingUserByUserName = await _userManager.FindByNameAsync(customer.Email);
        if (existingUserByUserName != null)
        {
            return new ResponseDto<Customers>
            {
                IsSuccess = false,
                Message = "User with this user already exists.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
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
            return new ResponseDto<Customers>
            {
                IsSuccess = false,
                Message = $"Registration failed: {errors}",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
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

        return new ResponseDto<Customers>
        {
            Id = user.Id,
            Token = token,
            IsSuccess = true,
            Message = "Register Successfully.",
        };


        // var sendMail = new MailRequest
        // {
        //     ToEmail = user.Email,
        //     Subject = "Confirm your email",
        //     Body = $"Please confirm your account by clicking this link: <br> <h3>{code}</h3>"
        // };

        // await SendEmailAsync(sendMail);

    }

    public async Task<ResponseDto<LoginDto>> Login(LoginDto login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);

        if (user == null)
        {
            return new ResponseDto<LoginDto>
            {
                IsSuccess = false,
                Message = "User not found with this email",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!result)
        {
            return new ResponseDto<LoginDto>
            {
                IsSuccess = false,
                Message = "Invalid Password.",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }

        var token = GenerateToken(user);

        return new ResponseDto<LoginDto>
        {
            Token = token,
            Id = user.Id,
            IsSuccess = true,
            Message = "Login Success.",
            HttpStatusCode = HttpStatusCode.OK,
        };
    }


    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();

        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

        email.Subject = mailRequest.Subject;

        var builder = new BodyBuilder();
        if (mailRequest.Attachments != null)
        {
            byte[] fileBytes;
            foreach (var file in mailRequest.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }

        builder.HtmlBody = mailRequest.Body;

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

        await smtp.SendAsync(email);

        smtp.Disconnect(true);
    }

    // public async Task<IActionResult> EmailVerification(string? id, string? code)
    // {
    //     if (id == null || code == null)
    //     {
    //         return new BadRequestObjectResult("Invalid payload");
    //     }

    //     var user = await _userManager.FindByIdAsync(id);

    //     if (user == null)
    //     {
    //         return new BadRequestObjectResult("Invalid payload");
    //     }

    //     var isVerified = await _userManager.ConfirmEmailAsync(user, code);

    //     if (isVerified.Succeeded)
    //     {
    //         return new OkObjectResult(new ResponseDto
    //         {
    //             IsSuccess = true,
    //             Message = "Email confirmed",
    //         });
    //     }

    //     return new BadRequestObjectResult("Something went wrong");
    // }

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
