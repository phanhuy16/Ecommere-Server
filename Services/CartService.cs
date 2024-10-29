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
            // Kiểm tra nếu giỏ hàng đã tồn tại dựa trên CreatedBy và ProductId
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.ProductId == carts.ProductId);

            if (existingCart != null)
            {
                // Nếu mục đã tồn tại, không thêm mới và trả về thông báo phù hợp
                return new Response<Cart>
                {
                    Data = existingCart,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartAlreadyExists"),
                    HttpStatusCode = HttpStatusCode.Conflict, // Mã trạng thái cho xung đột
                };
            }
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

}