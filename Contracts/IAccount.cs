using Microsoft.AspNetCore.Mvc;
using Server.Dtos;

namespace Server.Contracts;


public interface IAccount
{
    Task<ActionResult> Register(RegisterDto registerDto);

    Task<ActionResult<ResponseDto>> Login(LoginDto loginDto);

    Task<ActionResult<UserDetailDto>> GetUserDetail();

    Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers();
}