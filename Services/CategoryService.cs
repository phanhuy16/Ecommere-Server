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

    public async Task<Response<List<Category>>> GetAllCategories()
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.SupplierCategories)
                    .Select(cate => new Category
                    {
                        Id = cate.Id,
                        Title = cate.Title,
                        Slug = cate.Slug,
                        ParentId = cate.ParentId,
                        Description = cate.Description,
                        CreatedAt = cate.CreatedAt,
                        UpdatedAt = cate.UpdatedAt,
                        SupplierCategories = cate.SupplierCategories
                    })
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                await transaction.CommitAsync();

                return new Response<List<Category>>
                {
                    Data = categories,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "GetCategorySuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<List<Category>>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<IActionResult> GetPaginationCategories(PaginationFilter filter, string route)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var totalRecords = await _context.Categories.CountAsync();

                var categories = await _context.Categories
                    .Include(c => c.SupplierCategories)
                    .Select(cate => new Category
                    {
                        Id = cate.Id,
                        Title = cate.Title,
                        Slug = cate.Slug,
                        ParentId = cate.ParentId,
                        Description = cate.Description,
                        CreatedAt = cate.CreatedAt,
                        UpdatedAt = cate.UpdatedAt,
                        SupplierCategories = cate.SupplierCategories
                    })
                    .OrderByDescending(x => x.Id)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                var pagedResponse = PaginationHelper.CreatePagedReponse<Category>(categories, validFilter, totalRecords, _uriService, route);

                await transaction.CommitAsync();

                return new OkObjectResult(pagedResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message.ToString());
            }
        }
    }


    public async Task<Response<Category>> GetById(Guid CategoryId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (CategoryId == Guid.Empty)
                {
                    return new Response<Category>
                    {
                        Id = Guid.Empty,
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }
                var categories = await _context.Categories
                    .Include(c => c.SupplierCategories)
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
                        SupplierCategories = cate.SupplierCategories
                    })
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                await transaction.CommitAsync();

                return new Response<Category>
                {
                    Data = categories,
                    Id = CategoryId,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "GetCategorySuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Category>> Add(Category cate)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
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
                await transaction.CommitAsync();

                return new Response<Category>
                {
                    Data = category,
                    Id = category.Id,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CreateCategorySuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "CreateCategoryFailure") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Category>> Delete(Guid CategoryId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (CategoryId == Guid.Empty)
                {
                    return new Response<Category>
                    {
                        Id = Guid.Empty,
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var category = _context.Categories.Where(x => x.Id == CategoryId).FirstOrDefault();

                if (category == null)
                {
                    return new Response<Category>
                    {
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("CategoryMessages", "CategoryNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                // // Cập nhật các danh mục con (nếu có) để chuyển chúng thành danh mục cha
                // if (category.SubCategories != null && category.SubCategories.Any())
                // {
                //     foreach (var subCategory in category.SubCategories)
                //     {
                //         subCategory.ParentId = null; // Bỏ liên kết danh mục cha, chuyển thành danh mục cha
                //         _context.Categories.Update(subCategory);
                //     }
                // }

                // // Xóa tất cả các sản phẩm liên quan đến danh mục này (nếu cần)
                // if (category.Products != null && category.Products.Any())
                // {
                //     _context.Products.RemoveRange(category.Products);
                // }

                // Xóa danh mục
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Category>
                {
                    Data = category,
                    IsSuccess = true,
                    Id = CategoryId,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "DeleteCategorySuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "DeleteCategoryFailure") + " " +
            ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Category>> Update(Category cate, Guid CategoryId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (CategoryId == Guid.Empty)
                {
                    return new Response<Category>
                    {
                        Id = Guid.Empty,
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
                await transaction.CommitAsync();

                return new Response<Category>
                {
                    Id = CategoryId,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "UpdateCategorySuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                    Data = category
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Category>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("CategoryMessages", "UpdateCategoryFailure") + ": " + ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                };
            }
        }
    }
}