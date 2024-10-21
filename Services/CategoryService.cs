using Microsoft.EntityFrameworkCore;
using System.Net;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Pagination;
using Server.Utilities.Response;
using Microsoft.AspNetCore.Mvc;

namespace Server.Services;

public class CategoryService : ICategory
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    private readonly IUriService _uriService;
    public CategoryService(EFDataContext context, ApplicationSettings applicationSettings, IUriService uriService)
    {
        _context = context;
        _appSettings = applicationSettings;
        _uriService = uriService;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        try
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Select(cate => new Category
                {
                    Id = cate.Id,
                    Title = cate.Title,
                    Slug = cate.Slug,
                    ParentId = cate.ParentId,
                    Description = cate.Description,
                    CreatedAt = cate.CreatedAt,
                    UpdatedAt = cate.UpdatedAt,
                    SubCategories = cate.SubCategories.Select(sub => new Category
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Slug = sub.Slug,
                        ParentId = sub.ParentId,
                        Description = sub.Description,
                        CreatedAt = sub.CreatedAt,
                        UpdatedAt = sub.UpdatedAt
                    }).ToList()
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return categories;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<IActionResult> GetPaginationCategories(PaginationFilter filter, string route)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Categories.CountAsync();

            var categories = await _context.Categories
                .Include(c => c.SubCategories)  // Thêm Include để lấy các danh mục con
                .Select(cate => new Category
                {
                    Id = cate.Id,
                    Title = cate.Title,
                    Slug = cate.Slug,
                    ParentId = cate.ParentId,
                    Description = cate.Description,
                    CreatedAt = cate.CreatedAt,
                    UpdatedAt = cate.UpdatedAt,
                    SubCategories = cate.SubCategories.Select(sub => new Category
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Slug = sub.Slug,
                        ParentId = sub.ParentId,
                        Description = sub.Description,
                        CreatedAt = sub.CreatedAt,
                        UpdatedAt = sub.UpdatedAt
                    }).ToList()
                })
                .OrderByDescending(x => x.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var pagedResponse = PaginationHelper.CreatePagedReponse<Category>(categories, validFilter, totalRecords, _uriService, route);
            return new OkObjectResult(pagedResponse);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    public async Task<Category> GetById(Guid CategoryId)
    {
        try
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.Id == CategoryId)
                .Select(cate => new Category
                {
                    Id = cate.Id,
                    Title = cate.Title,
                    Slug = cate.Slug,
                    ParentId = cate.ParentId,
                    Description = cate.Description,
                    CreatedAt = cate.CreatedAt,
                    UpdatedAt = cate.UpdatedAt,
                    SubCategories = cate.SubCategories.Select(sub => new Category
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Slug = sub.Slug,
                        ParentId = sub.ParentId,
                        Description = sub.Description,
                        CreatedAt = sub.CreatedAt,
                        UpdatedAt = sub.UpdatedAt
                    }).ToList()
                })
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            return categories;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    public async Task<Response<Category>> Add(Category cate)
    {
        try
        {
            // Kiểm tra xem Category có tồn tại không (dựa trên Title và ParentId)
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Title == cate.Title && c.ParentId == cate.ParentId);

            if (existingCategory != null)
            {
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Kiểm tra xem Slug đã tồn tại chưa
            var existingSlug = await _context.Categories
                .AnyAsync(c => c.Slug == cate.Slug);

            if (existingSlug)
            {
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryAlreadyExists"),
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }


            // Nếu ParentId không null, kiểm tra xem danh mục cha có tồn tại không
            if (cate.ParentId.HasValue)
            {
                var parentCategoryExists = await _context.Categories.AnyAsync(c => c.Id == cate.ParentId.Value);
                if (!parentCategoryExists)
                {
                    return new Response<Category>
                    {
                        IsSuccess = false,
                        Message = $"Sub category with ID {cate.ParentId} does not exist.",
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            var category = new Category()
            {
                Title = cate.Title,
                Slug = cate.Slug,
                ParentId = cate.ParentId,
                Description = cate.Description,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new Response<Category>
            {
                Data = category,
                CategoryId = category.Id,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("CategoryMessages", "CreateCategorySuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the category: {ex.Message}");
        }
    }
    public async Task<Response<Category>> Delete(Guid CategoryId)
    {
        try
        {
            if (CategoryId == Guid.Empty)
            {
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            var category = _context.Categories.Include(c => c.ProductCategories).Include(c => c.SubCategories).Where(x => x.Id == CategoryId).FirstOrDefault();
            if (category == null)
            {
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Cập nhật các danh mục con (nếu có) để chuyển chúng thành danh mục cha
            if (category.SubCategories != null && category.SubCategories.Any())
            {
                foreach (var subCategory in category.SubCategories)
                {
                    subCategory.ParentId = null; // Bỏ liên kết danh mục cha, chuyển thành danh mục cha
                    _context.Categories.Update(subCategory);
                }
            }

            // Xóa tất cả các sản phẩm liên quan đến danh mục này (nếu cần)
            if (category.ProductCategories != null && category.ProductCategories.Any())
            {
                _context.ProductCategories.RemoveRange(category.ProductCategories);
            }

            // Xóa danh mục
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new Response<Category>
            {
                Data = category,
                IsSuccess = true,
                CategoryId = CategoryId,
                Message = _appSettings.GetConfigurationValue("CategoryMessages", "DeleteCategorySuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Category>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("CategoryMessages", "DeleteCategoryFailure") + " " +
            ex.Message.ToString(),
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }
    public async Task<Response<Category>> Update(Category cate, Guid CategoryId)
    {
        try
        {
            if (CategoryId == Guid.Empty)
            {
                return new Response<Category>
                {
                    CategoryId = CategoryId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == CategoryId);

            if (category == null)
            {
                return new Response<Category>
                {
                    CategoryId = CategoryId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }



            // Update properties
            category.Title = cate.Title;
            category.Slug = cate.Slug;

            // Only update ParentId if provided
            if (cate.ParentId != null && cate.ParentId != Guid.Empty)
            {
                var parentExists = await _context.Categories.AnyAsync(c => c.Id == cate.ParentId);
                if (!parentExists)
                {
                    return new Response<Category>
                    {
                        IsSuccess = false,
                        Message = $"Parent category with ID {cate.ParentId} does not exist.",
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }
                category.ParentId = cate.ParentId;
            }

            category.Description = cate.Description;
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new Response<Category>
            {
                CategoryId = CategoryId,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("CategoryMessages", "UpdateCategorySuccess"),
                HttpStatusCode = HttpStatusCode.OK,
                Data = category
            };
        }
        catch (Exception ex)
        {
            return new Response<Category>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("CategoryMessages", "UpdateCategoryFailure") + ": " + ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}