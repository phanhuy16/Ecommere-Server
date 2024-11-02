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
        using (var transaction = await _context.Database.BeginTransactionAsync())
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
                await transaction.CommitAsync();

                return new Response<Promotion>
                {
                    Data = promotion,
                    Id = promotion.Id,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "CreatePromotionSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Promotion>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }


    public async Task<Response<List<Promotion>>> GetAllPromotion()
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
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

                await transaction.CommitAsync();

                return new Response<List<Promotion>>
                {
                    Data = promotion,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "GetPromotionSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<List<Promotion>>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Promotion>> UpdatePromotion(Promotion promo, Guid Id)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (Id <= Guid.Empty)
                {
                    return new Response<Promotion>
                    {
                        IsSuccess = false,
                        Id = Guid.Empty,
                        Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var promotion = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == Id);
                if (promotion == null)
                {
                    return new Response<Promotion>
                    {
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                // Update properties
                promotion.Title = promo.Title;
                promotion.Description = promo.Description;
                promotion.Code = promo.Code;
                promotion.Value = promo.Value;
                promotion.NumOfAvailable = promo.NumOfAvailable;
                promotion.Type = promotion.Type;
                promotion.ImageURL = promo.ImageURL;
                promotion.StartAt = promo.StartAt;
                promotion.EndAt = promo.EndAt;

                _context.Promotions.Update(promotion);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Promotion>
                {
                    Data = promotion,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "UpdatePromotionSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Promotion>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Promotion>> DeletePromotion(Guid Id)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (Id <= Guid.Empty)
                {
                    return new Response<Promotion>
                    {
                        IsSuccess = false,
                        Id = Guid.Empty,
                        Message = "Id Not Found",
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }
                var promotion = _context.Promotions.Where(x => x.Id == Id).FirstOrDefault();
                if (promotion == null)
                {
                    return new Response<Promotion>
                    {
                        IsSuccess = false,
                        Message = "Promotion Not Found",
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Promotion>
                {
                    IsSuccess = true,
                    Id = Id,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "DeletePromotionSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Promotion>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }
}