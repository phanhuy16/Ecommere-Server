using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Dtos.Requests;
using Server.Utilities.Response;

namespace Server.Contracts;


public interface IAccount
{
    Task<Response<object>> Register(Register register);

    Task<Response<object>> Login(Login login);
    Task<ResponseDTO> RefreshToken(TokenRequest tokenRequest);
}