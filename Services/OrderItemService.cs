

using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;

namespace Server.Services;

public class OrderItemService : IOrderItem
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    public OrderItemService(EFDataContext context, ApplicationSettings applicationSettings)
    {
        _context = context;
        _appSettings = applicationSettings;
    }
    public async Task<List<OrderItem>> GetAll()
    {
        try
        {
            var orderitem = await _context.OrderItems.Select(item => new OrderItem
            {
                Product_Id = item.Product_Id,
                Inventory_Id = item.Inventory_Id,
                Report_Id = item.Report_Id,
                Ordered_Quantity = item.Ordered_Quantity,
                Created_At = item.Created_At,
                Updated_At = item.Updated_At,
            }).OrderByDescending(x => x.Order_Id).ToListAsync();
            return orderitem;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<OrderItem> GetById(Guid OrderItemId)
    {
        try
        {
            var orderitem = await (from item in _context.OrderItems
                                   select new OrderItem
                                   {
                                       Product_Id = item.Product_Id,
                                       Inventory_Id = item.Inventory_Id,
                                       Report_Id = item.Report_Id,
                                       Ordered_Quantity = item.Ordered_Quantity,
                                       Created_At = item.Created_At,
                                       Updated_At = item.Updated_At,
                                   }).FirstOrDefaultAsync();
            return orderitem;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
}