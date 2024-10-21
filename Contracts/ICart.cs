

using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface ICart
{
    Task<List<Cart>> GetAll();
    Task<Cart> GetById(Guid CartId);
    Task<Response<Cart>> Add(Cart cart);
    Task<Response<Cart>> Update(Cart cart, Guid CartId);
    Task<Response<Cart>> Delete(Guid CartId);
}