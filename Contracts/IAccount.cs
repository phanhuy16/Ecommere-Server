using Microsoft.AspNetCore.Mvc;
using Server.Dtos;

namespace Server.Contracts;


public interface IAccount
{
    Task<ResponseDto<RegisterDto>> Register(RegisterDto registerDto);

    Task<ResponseDto<LoginDto>> Login(LoginDto loginDto);

    Task<ResponseDto<UserDetailDto>> GetUserDetail();

    Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers();
}