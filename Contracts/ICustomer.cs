

using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Entities;

namespace Server.Contracts;
public interface ICustomer
{
    Task<ResponseDto<Customers>> RegisterCustomer(Customers customer);
    // Task<IActionResult> EmailVerification(string? email, string? code);
    Task<ResponseDto<LoginDto>> Login(LoginDto login);
    Task SendEmailAsync(MailRequest mailRequest);

}
