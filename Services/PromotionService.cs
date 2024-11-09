using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
                    Slug = promo.Slug,
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
                    Slug = promo.Slug,
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
                promotion.Slug = promo.Slug;
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

    public async Task<Response<FileContentResult>> ExportExcel()
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var promotion = await _context.Promotions.ToListAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Promotion");

                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Title";
                    worksheet.Cells[1, 3].Value = "Slug";
                    worksheet.Cells[1, 4].Value = "Description";
                    worksheet.Cells[1, 5].Value = "Code";
                    worksheet.Cells[1, 6].Value = "Value";
                    worksheet.Cells[1, 7].Value = "NumOfAvailable";
                    worksheet.Cells[1, 8].Value = "Type";
                    worksheet.Cells[1, 9].Value = "StartAt";
                    worksheet.Cells[1, 10].Value = "EndAt";
                    worksheet.Cells[1, 11].Value = "ImageURL";

                    for (int i = 0; i < promotion.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = promotion[i].Id;
                        worksheet.Cells[i + 2, 2].Value = promotion[i].Title;
                        worksheet.Cells[i + 2, 3].Value = promotion[i].Slug;
                        worksheet.Cells[i + 2, 4].Value = promotion[i].Description;
                        worksheet.Cells[i + 2, 5].Value = promotion[i].Code;
                        worksheet.Cells[i + 2, 6].Value = promotion[i].Value;
                        worksheet.Cells[i + 2, 7].Value = promotion[i].NumOfAvailable;
                        worksheet.Cells[i + 2, 8].Value = promotion[i].Type;
                        worksheet.Cells[i + 2, 9].Value = promotion[i].StartAt;
                        worksheet.Cells[i + 2, 10].Value = promotion[i].EndAt;
                        worksheet.Cells[i + 2, 11].Value = promotion[i].ImageURL;
                    }

                    // Thiết lập response để trả về file Excel
                    var fileName = "Promotion.xlsx";
                    var fileContent = package.GetAsByteArray();

                    var result = new FileContentResult(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = fileName
                    };

                    await transaction.CommitAsync();

                    return new Response<FileContentResult>
                    {
                        Data = result,
                        IsSuccess = true,
                        Message = _appSettings.GetConfigurationValue("PromotionMessages", "GetPromotionSuccess"),
                        HttpStatusCode = HttpStatusCode.OK,
                    };

                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<FileContentResult>()
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "PromotionNotFound") + " " +
            ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<IEnumerable<Promotion>>> Search(string slug)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                IQueryable<Promotion> query = _context.Promotions;

                if (!string.IsNullOrEmpty(slug))
                {
                    query = query.Where(e => e.Slug.Contains(slug)
                                || e.Title.Contains(slug));
                }

                await transaction.CommitAsync();

                return new Response<IEnumerable<Promotion>>()
                {
                    Data = await query.ToListAsync(),
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("PromotionMessages", "GetPromotionSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<IEnumerable<Promotion>>()
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