using Microsoft.AspNetCore.Identity;
using Server.Entities;
using Server.Utilities.Response;


namespace Server.Contracts;

public interface ISetUp
{
    Task<Response<List<IdentityRole>>> GetAllRoles();
    Task<Response<List<User>>> GetAllUsers();
    Task<Response<string>> CreateRole(string name);
    Task<Response<string>> AddUserToRole(string email, string roleName);
    Task<Response<IList<string>>> GetUserRoles(string email);
    Task<Response<string>> RemoveUserFromRole(string email, string roleName);
}