using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Response;

namespace Server.Services;

public class OrderService : IOrder
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    public OrderService(EFDataContext context, ApplicationSettings applicationSettings)
    {
        _context = context;
        _appSettings = applicationSettings;
    }

    // public async Task<Response<List<Order>>> GetAll()
    // {
    //     try
    //     {
    //         var order = await _context.Orders.Select( new Order
    //                            {

    //                            }).OrderByDescending(x => x.Order_Id).ToListAsync();
    //         return new Response<List<Order>>
    //         {
    //             Data = order,
    //             IsSuccess = true,
    //             Message = "Get all promotion successfully.",
    //             HttpStatusCode = HttpStatusCode.OK,
    //         };

    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.Message.ToString());
    //     }
    // }

    // public async Task<Order> GetById(Guid OrderId)
    // {
    //     try
    //     {
    //         var order = await (from or in _context.Orders
    //                            join cart in _context.Carts
    //                            on or.Cart_Id equals cart.Cart_Id
    //                            where or.Order_Id == OrderId
    //                            select new Order
    //                            {
    //                                Cart_Id = or.Cart_Id,
    //                                Order_Date = or.Order_Date,
    //                                Order_Decs = or.Order_Decs,
    //                                Order_Fee = or.Order_Fee,
    //                                Created_At = or.Created_At,
    //                                Updated_At = or.Updated_At,
    //                            }).FirstOrDefaultAsync();
    //         return order;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.Message.ToString());
    //     }
    // }

    // public async Task<Response<Order>> Add(Order or)
    // {
    //     try
    //     {
    //         var order = new Order()
    //         {
    //             Cart_Id = or.Cart_Id,
    //             Order_Date = or.Order_Date,
    //             Order_Decs = or.Order_Decs,
    //             Order_Fee = or.Order_Fee,
    //             Created_At = or.Created_At,
    //             Updated_At = or.Updated_At,
    //             Cart = _context.Carts.Find(or.Cart_Id)
    //         };

    //         await _context.Orders.AddAsync(order);
    //         await _context.SaveChangesAsync();

    //         return new Response<Order>
    //         {
    //             OrderId = order.Order_Id,
    //             IsSuccess = true,
    //             Message = _appSettings.GetConfigurationValue("OrderMessages", "CreateOrderSuccess"),
    //             HttpStatusCode = HttpStatusCode.OK,
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"An error occurred while adding the Order: {ex.Message}");
    //     }
    // }

    // public async Task<Response<Order>> Delete(Guid OrderId)
    // {
    //     try
    //     {
    //         if (OrderId <= Guid.Empty)
    //         {
    //             return new Response<Order>
    //             {
    //                 IsSuccess = false,
    //                 Message = _appSettings.GetConfigurationValue("OrderMessages", "OrderNotFound"),
    //                 HttpStatusCode = HttpStatusCode.BadRequest,
    //             };
    //         }
    //         var order = _context.Orders.Where(x => x.Order_Id == OrderId).FirstOrDefault();
    //         if (order == null)
    //         {
    //             return new Response<Order>
    //             {
    //                 IsSuccess = false,
    //                 Message = _appSettings.GetConfigurationValue("OrderMessages", "OrderNotFound"),
    //                 HttpStatusCode = HttpStatusCode.BadRequest,
    //             };
    //         }
    //         _context.Orders.Remove(order);
    //         await _context.SaveChangesAsync();
    //         return new Response<Order>
    //         {
    //             IsSuccess = true,
    //             OrderId = OrderId,
    //             Message = _appSettings.GetConfigurationValue("OrderMessages", "DeleteOrderSuccess"),
    //             HttpStatusCode = HttpStatusCode.OK,
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new Response<Order>
    //         {
    //             IsSuccess = false,
    //             Message = _appSettings.GetConfigurationValue("OrderMessages", "DeleteOrderFailure") + " " +
    //         ex.Message.ToString(),
    //             HttpStatusCode = HttpStatusCode.BadRequest,
    //         };
    //     }
    // }

    // public async Task<Response<Order>> Update(Order or, Guid OrderId)
    // {
    //     try
    //     {
    //         if (OrderId <= Guid.Empty)
    //         {
    //             return new Response<Order>
    //             {
    //                 OrderId = OrderId,
    //                 IsSuccess = false,
    //                 Message = _appSettings.GetConfigurationValue("OrderMessages", "OrderNotFound"),
    //                 HttpStatusCode = HttpStatusCode.BadRequest,
    //             };
    //         }

    //         var order = await _context.Orders.FirstOrDefaultAsync(x => x.Order_Id == OrderId);
    //         if (order == null)
    //         {
    //             return new Response<Order>
    //             {
    //                 OrderId = OrderId,
    //                 IsSuccess = false,
    //                 Message = _appSettings.GetConfigurationValue("OrderMessages", "OrderNotFound"),
    //                 HttpStatusCode = HttpStatusCode.BadRequest,
    //             };
    //         }

    //         // Update properties
    //         order.Cart_Id = or.Cart_Id;
    //         order.Order_Date = or.Order_Date;
    //         order.Order_Decs = or.Order_Decs;
    //         order.Order_Fee = or.Order_Fee;
    //         order.Created_At = or.Created_At;
    //         order.Cart = _context.Carts.Find(or.Cart_Id);

    //         _context.Orders.Update(order);
    //         await _context.SaveChangesAsync();

    //         return new Response<Order>
    //         {
    //             OrderId = order.Order_Id,
    //             IsSuccess = true,
    //             Message = _appSettings.GetConfigurationValue("OrderMessages", "UpdateOrderSuccess"),
    //             HttpStatusCode = HttpStatusCode.OK,
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new Response<Order>
    //         {
    //             IsSuccess = false,
    //             Message = _appSettings.GetConfigurationValue("OrderMessages", "UpdateOrderFailure") + ": " + ex.Message,
    //             HttpStatusCode = HttpStatusCode.InternalServerError,
    //         };
    //     }
    // }

}