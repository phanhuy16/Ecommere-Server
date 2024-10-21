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

    public async Task<List<Cart>> GetAll()
    {
        try
        {
            var cart = await (from car in _context.Carts
                              join user in _context.Users
                              on car.User_Id equals user.Id
                              select new Cart
                              {
                                  Cart_Id = car.Cart_Id,
                                  User_Id = car.User_Id,
                                  Created_At = car.Created_At,
                                  Updated_At = car.Updated_At,
                              }).OrderByDescending(x => x.Cart_Id).ToListAsync();
            return cart;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<Cart> GetById(Guid CartId)
    {
        try
        {
            var cart = (from car in _context.Carts
                        join user in _context.Users
                        on car.User_Id equals user.Id
                        where car.Cart_Id == CartId
                        select new Cart
                        {
                            Cart_Id = car.Cart_Id,
                            User_Id = car.User_Id,
                            Created_At = car.Created_At,
                            Updated_At = car.Updated_At,
                        }).FirstOrDefaultAsync();
            return await cart;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<Response<Cart>> Add(Cart cart)
    {
        try
        {
            var carts = new Cart()
            {
                Cart_Id = cart.Cart_Id,
                User_Id = cart.User_Id,
                Created_At = cart.Created_At,
                Updated_At = cart.Updated_At,
                User = _context.Users.Find(cart.User_Id)
            };

            await _context.Carts.AddAsync(carts);
            await _context.SaveChangesAsync();

            return new Response<Cart>
            {
                CartId = carts.Cart_Id,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("CartMessages", "CreateCartSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the cart: {ex.Message}");
        }
    }

    public async Task<Response<Cart>> Delete(Guid CartId)
    {
        try
        {
            if (CartId <= Guid.Empty)
            {
                return new Response<Cart>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            var cart = _context.Carts.Where(x => x.Cart_Id == CartId).FirstOrDefault();
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
                CartId = CartId,
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

    public async Task<Response<Cart>> Update(Cart cart, Guid CartId)
    {
        try
        {
            if (CartId <= Guid.Empty)
            {
                return new Response<Cart>
                {
                    CartId = CartId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            var carts = await _context.Carts.FirstOrDefaultAsync(x => x.Cart_Id == CartId);
            if (carts == null)
            {
                return new Response<Cart>
                {
                    CartId = CartId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CartMessages", "CartNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Update properties
            carts.User_Id = cart.User_Id;
            carts.Updated_At = cart.Updated_At;
            carts.User = _context.Users.Find(cart.User_Id);

            _context.Carts.Update(carts);
            await _context.SaveChangesAsync();

            return new Response<Cart>
            {
                CartId = CartId,
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
                Message = _appSettings.GetConfigurationValue("CartMessages", "UpdateCartFailure") + ": " + ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}