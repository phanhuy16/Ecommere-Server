
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface IOrder
{
    Task<List<Order>> GetAll();
    Task<Order> GetById(Guid OrderId);
    Task<Response<Order>> Add(Order or);
    Task<Response<Order>> Update(Order or, Guid OrderId);
    Task<Response<Order>> Delete(Guid OrderId);
}