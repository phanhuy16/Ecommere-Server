

using Microsoft.AspNetCore.Mvc;
using Server.Dtos;

namespace Server.Contracts;
public interface ICustomer
{
    Task<ActionResult> RegisterCustomer(Customers customer);
}
