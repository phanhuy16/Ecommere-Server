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
                Image = carts.Image,
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
                Id = Id,
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
    public async Task<Response<List<Cart>>> GetCatItem(string createdBy)
    {
        try
        {
            var cart = await _context.Carts
                .Where(car => car.CreatedBy == createdBy) // Lọc theo CreatedBy
                .Select(car => new Cart
                {
                    Id = car.Id,
                    CreatedBy = car.CreatedBy,
                    Count = car.Count,
                    Size = car.Size,
                    Color = car.Color,
                    Price = car.Price,
                    Qty = car.Qty,
                    SubProductId = car.SubProductId,
                    ProductId = car.ProductId,
                    Image = car.Image,
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return new Response<List<Cart>>
            {
                Data = cart,
                IsSuccess = true,
                Message = "Get all cart items successfully.",
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            // Thay vì ném một exception mới, bạn có thể trả về một Response có lỗi
            return new Response<List<Cart>>
            {
                Data = null,
                IsSuccess = false,
                Message = ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }

    public async Task<Response<Cart>> UpdateCartQuantity(Guid Id, int additionalCount)
    {
        try
        {
            // Kiểm tra nếu Id giỏ hàng hợp lệ
            if (Id == Guid.Empty)
            {
                return new Response<Cart>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "InvalidCartId"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Tìm giỏ hàng dựa trên Id
            var cart = await _context.Carts.FindAsync(Id);

            if (cart == null)
            {
                return new Response<Cart>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.NotFound,
                };
            }

            // Cập nhật giá trị Count hiện tại với additionalCount
            cart.Count += additionalCount;

            // Lưu thay đổi
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return new Response<Cart>
            {
                Data = cart,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("CartMessages", "UpdateCartSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Cart>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("CartMessages", "UpdateCartFailure") + " " + ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}