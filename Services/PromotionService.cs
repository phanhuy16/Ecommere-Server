using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Response;

namespace Server.Services;
public class PromotionService : IPromotion
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    public PromotionService(EFDataContext context, ApplicationSettings applicationSettings)
    {
        _context = context;
        _appSettings = applicationSettings;
    }

    public async Task<Response<Promotion>> AddPromotion(Promotion promo)
    {
        try
        {
            var promotion = new Promotion()
            {
                Id = promo.Id,
                Title = promo.Title,
                Description = promo.Description,
                Code = promo.Code,
                Type = promo.Type,
                Value = promo.Value,
                ImageURL = promo.ImageURL,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow,
            };

            await _context.AddAsync(promotion);
            await _context.SaveChangesAsync();

            return new Response<Promotion>
            {
                Data = promotion,
                IsSuccess = true,
                Message = "Add new promotion successfully.",
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (System.Exception)
        {

            return new Response<Promotion>
            {
                IsSuccess = false,
                Message = "Error server",
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }


    public async Task<Response<List<Promotion>>> GetAllPromotion()
    {
        try
        {
            var promotion = await _context.Promotions
            .Select(promo => new Promotion
            {
                Id = promo.Id,
                Title = promo.Title,
                Description = promo.Description,
                Code = promo.Code,
                Value = promo.Value,
                NumOfAvailable = promo.NumOfAvailable,
                Type = promo.Type,
                ImageURL = promo.ImageURL,
                StartAt = promo.StartAt,
                EndAt = promo.EndAt,
            })
            .OrderByDescending(x => x.Id)
            .ToListAsync();

            var query = promotion.ToList<Promotion>();

            return new Response<List<Promotion>>
            {
                Data = query,
                IsSuccess = true,
                Message = "Get all promotion successfully.",
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
}
