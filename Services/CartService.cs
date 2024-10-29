using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Response;
using System.Net;

namespace Server.Services;

public class CartService : ICart
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    public CartService(EFDataContext context, ApplicationSettings applicationSettings)
    {
        _context = context;
        _appSettings = applicationSettings;
    }

    public async Task<Response<Cart>> AddMultiple(Cart carts)
    {
        try
        {
            var cart = new Cart()
            {
                Id = carts.Id,
                CreatedBy = carts.CreatedBy,
                Count = carts.Count,
                Size = carts.Size,
                Color = carts.Color,
                Price = carts.Price,
                Qty = carts.Qty,
                SubProductId = carts.SubProductId,
                ProductId = carts.ProductId,
            };

            // Thêm toàn bộ danh sách carts vào cơ sở dữ liệu
            await _context.Carts.AddRangeAsync(cart);
            await _context.SaveChangesAsync();

            return new Response<Cart>
            {
                Data = cart,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("CartMessages", "CreateCartSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the carts: {ex.Message}");
        }
    }



    public async Task<Response<Cart>> Delete(Guid Id)
    {
        try
        {
            if (Id <= Guid.Empty)
            {
                return new Response<Cart>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            var cart = _context.Carts.Where(x => x.Id == Id).FirstOrDefault();
            if (cart == null)
            {
                return new Response<Cart>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return new Response<Cart>
            {
                IsSuccess = true,
                CartId = Id,
                Message = _appSettings.GetConfigurationValue("CartMessages", "DeleteCartSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Cart>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("CartMessages", "DeleteCartFailure") + " " +
            ex.Message.ToString(),
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }


}