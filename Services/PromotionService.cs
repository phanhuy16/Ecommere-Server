using System.Net;
using Microsoft.AspNetCore.Mvc;
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
}
