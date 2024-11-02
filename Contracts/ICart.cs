

using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface ICart
{
    Task<Response<Cart>> AddMultiple(Cart carts);
    Task<Response<Cart>> Delete(Guid Id);
    Task<Response<List<Cart>>> GetCatItem(string createdBy);
    Task<Response<Cart>> UpdateCartQuantity(Guid Id, int additionalCount);
}