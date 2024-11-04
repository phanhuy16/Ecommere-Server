using System.Security.Claims;
using Server.Utilities.Response;

namespace Server.Contracts;
public interface IClaimsSetup
{
    Task<Response<IList<Claim>>> GetAllClaims(string email);
    Task<Response<object>> AddClaimsToUser(string email, string claimName, string claimValue);
}
