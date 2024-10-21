

using Server.Entities;

namespace Server.Contracts;

public interface IOrderItem
{
    Task<List<OrderItem>> GetAll();
    Task<OrderItem> GetById(Guid OrderItemId);
    // Task<Response> Add(OrderItem or);
    // Task<Response> UpdateMany(OrderItem or);
    // Task<Response> Update(OrderItem or, int OrderItemId);
    // Task<Response> Delete(int OrderItemId);
}