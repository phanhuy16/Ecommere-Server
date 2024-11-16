
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Pagination;
using Server.Utilities.Response;

namespace Server.Services;


public class SupplierService : ISupplier
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    private readonly IUriService _uriService;
    public SupplierService(EFDataContext context, ApplicationSettings applicationSettings, IUriService uriService)
    {
        _context = context;
        _appSettings = applicationSettings;
        _uriService = uriService;
    }

    public async Task<Response<Supplier>> Post(Supplier sup)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var supplier = new Supplier()
                {
                    Name = sup.Name,
                    Slug = sup.Slug,
                    Price = sup.Price,
                    Contact = sup.Contact,
                    IsTalking = sup.IsTalking,
                    Email = sup.Email,
                    Active = sup.Active,
                    Image = sup.Image,
                    CreatedAt = DateTime.UtcNow,
                    Product = sup.Product,
                };

                await _context.Suppliers.AddAsync(supplier);

                if (sup.SupplierCategory != null && sup.SupplierCategory.Any())
                {
                    foreach (var category in sup.SupplierCategory)
                    {
                        var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.CategoryId);

                        // Kiểm tra danh mục có tồn tại không
                        if (existingCategory == null)
                        {
                            // Nếu danh mục không hợp lệ, rollback transaction
                            await transaction.RollbackAsync();

                            return new Response<Supplier>
                            {
                                IsSuccess = false,
                                Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                                HttpStatusCode = HttpStatusCode.BadRequest,
                            };
                        }

                        var supplierCategories = new SupplierCategory
                        {
                            SupplierId = supplier.Id,
                            CategoryId = category.CategoryId
                        };
                        await _context.SupplierCategories.AddAsync(supplierCategories);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Supplier>
                {
                    Data = supplier,
                    Id = supplier.Id,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "CreateSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Supplier>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "CreateSupplierFailure") + " " + ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Supplier>> Update(Supplier sup, Guid SupplierId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (SupplierId == Guid.Empty)
                {
                    return new Response<Supplier>
                    {
                        Id = Guid.Empty,
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var supplier = _context.Suppliers.Where(x => x.Id == SupplierId).FirstOrDefault();

                if (supplier == null)
                {
                    return new Response<Supplier>
                    {
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                supplier.Id = sup.Id;
                supplier.Name = sup.Name;
                supplier.Slug = sup.Slug;
                supplier.Price = sup.Price;
                supplier.Contact = sup.Contact;
                supplier.IsTalking = sup.IsTalking;
                supplier.Email = sup.Email;
                supplier.Active = sup.Active;
                supplier.Image = sup.Image;
                supplier.UpdatedAt = DateTime.UtcNow;
                supplier.Product = sup.Product;

                _context.Suppliers.Update(supplier);

                var existingCategoryIds = supplier.SupplierCategory.Select(pc => pc.CategoryId).ToList();

                var newCategoryIds = sup.SupplierCategory.Select(pc => pc.CategoryId).ToList();

                if (!existingCategoryIds.SequenceEqual(newCategoryIds))
                {
                    var existingSupplierCategories = _context.SupplierCategories.Where(pc => pc.SupplierId == SupplierId).ToList();
                    _context.SupplierCategories.RemoveRange(existingSupplierCategories);

                    foreach (var categoryId in newCategoryIds)
                    {
                        var supplierCategory = new SupplierCategory
                        {
                            SupplierId = supplier.Id,
                            CategoryId = categoryId
                        };
                        await _context.SupplierCategories.AddAsync(supplierCategory);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Supplier>
                {
                    Data = supplier,
                    Id = SupplierId,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "UpdateSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Supplier>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "UpdateSupplierFailure") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<Supplier>> Delete(Guid SupplierId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (SupplierId == Guid.Empty)
                {
                    return new Response<Supplier>
                    {
                        Id = Guid.Empty,
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }
                var supplier = _context.Suppliers.Where(x => x.Id == SupplierId).FirstOrDefault();
                if (supplier == null)
                {
                    return new Response<Supplier>
                    {
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response<Supplier>
                {
                    Data = supplier,
                    IsSuccess = true,
                    Id = SupplierId,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "DeleteSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Supplier>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "DeleteSupplierFailure") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<IActionResult> GetPaginationSupplier(PaginationFilter filter, string route)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var totalRecords = await _context.Suppliers.CountAsync();

                var supplier = await _context.Suppliers
                                .Include(c => c.SupplierCategory)
                                .Select(sup => new Supplier
                                {
                                    Id = sup.Id,
                                    Name = sup.Name,
                                    Slug = sup.Slug,
                                    Price = sup.Price,
                                    Contact = sup.Contact,
                                    IsTalking = sup.IsTalking,
                                    Email = sup.Email,
                                    Active = sup.Active,
                                    Image = sup.Image,
                                    Product = sup.Product,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    SupplierCategory = sup.SupplierCategory.Select(sc => new SupplierCategory
                                    {
                                        Category = new Category()
                                        {
                                            Id = sc.Category.Id,
                                            Title = sc.Category.Title,
                                        }
                                    }).ToList(),
                                })
                                .OrderByDescending(x => x.Id)
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

                var pagedResponse = PaginationHelper.CreatePagedReponse<Supplier>(supplier, validFilter, totalRecords, _uriService, route);

                await transaction.CommitAsync();

                return new OkObjectResult(pagedResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.ToString());
            }
        }
    }

    public async Task<Response<List<Supplier>>> GetAllSupplier()
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var supplier = await _context.Suppliers
                                .Include(c => c.SupplierCategory)
                                .Select(sup => new Supplier
                                {
                                    Id = sup.Id,
                                    Name = sup.Name,
                                    Slug = sup.Slug,
                                    Price = sup.Price,
                                    Contact = sup.Contact,
                                    IsTalking = sup.IsTalking,
                                    Email = sup.Email,
                                    Active = sup.Active,
                                    Image = sup.Image,
                                    Product = sup.Product,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    SupplierCategory = sup.SupplierCategory.Select(sc => new SupplierCategory
                                    {
                                        Category = new Category()
                                        {
                                            Id = sc.Category.Id,
                                            Title = sc.Category.Title,
                                        }
                                    }).ToList(),
                                }).OrderByDescending(x => x.Id).ToListAsync();

                await transaction.CommitAsync();

                return new Response<List<Supplier>>
                {
                    Data = supplier,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "GetSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<List<Supplier>>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound") + " " +
                ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }
    public async Task<Response<Supplier>> GetById(Guid SupplierId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (SupplierId == Guid.Empty)
                {
                    return new Response<Supplier>
                    {
                        Id = Guid.Empty,
                        IsSuccess = false,
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var supplier = await _context.Suppliers
                                .Include(c => c.SupplierCategory)
                                .Select(sup => new Supplier
                                {
                                    Id = sup.Id,
                                    Name = sup.Name,
                                    Slug = sup.Slug,
                                    Price = sup.Price,
                                    Contact = sup.Contact,
                                    IsTalking = sup.IsTalking,
                                    Email = sup.Email,
                                    Active = sup.Active,
                                    Product = sup.Product,
                                    Image = sup.Image,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    SupplierCategory = sup.SupplierCategory.Select(sc => new SupplierCategory
                                    {
                                        Category = new Category()
                                        {
                                            Id = sc.Category.Id,
                                            Title = sc.Category.Title,
                                        }
                                    }).ToList(),
                                }).FirstOrDefaultAsync();

                await transaction.CommitAsync();

                return new Response<Supplier>
                {
                    Data = supplier,
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "GetSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<Supplier>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound") + " " +
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
                var supplier = await _context.Suppliers.ToListAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Supplier");

                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Name";
                    worksheet.Cells[1, 3].Value = "Slug";
                    worksheet.Cells[1, 4].Value = "Price";
                    worksheet.Cells[1, 5].Value = "Product";
                    worksheet.Cells[1, 6].Value = "Contact";
                    worksheet.Cells[1, 7].Value = "IsTalking";
                    worksheet.Cells[1, 8].Value = "Email";
                    worksheet.Cells[1, 9].Value = "Active";
                    worksheet.Cells[1, 10].Value = "Image";
                    worksheet.Cells[1, 11].Value = "CreatedAt";
                    worksheet.Cells[1, 12].Value = "UpdatedAt";
                    worksheet.Cells[1, 13].Value = "Categories";

                    for (int i = 0; i < supplier.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = supplier[i].Id;
                        worksheet.Cells[i + 2, 2].Value = supplier[i].Name;
                        worksheet.Cells[i + 2, 3].Value = supplier[i].Slug;
                        worksheet.Cells[i + 2, 4].Value = supplier[i].Price;
                        worksheet.Cells[i + 2, 5].Value = supplier[i].Product;
                        worksheet.Cells[i + 2, 6].Value = supplier[i].Contact;
                        worksheet.Cells[i + 2, 7].Value = supplier[i].IsTalking;
                        worksheet.Cells[i + 2, 8].Value = supplier[i].Email;
                        worksheet.Cells[i + 2, 9].Value = supplier[i].Active;
                        worksheet.Cells[i + 2, 10].Value = supplier[i].Image;
                        worksheet.Cells[i + 2, 11].Value = supplier[i].CreatedAt;
                        worksheet.Cells[i + 2, 12].Value = supplier[i].UpdatedAt;
                        worksheet.Cells[i + 2, 13].Value = supplier[i].SupplierCategory;
                    }

                    // Thiết lập response để trả về file Excel
                    var fileName = "Supplier.xlsx";
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
                        Message = _appSettings.GetConfigurationValue("SupplierMessages", "GetSupplierSuccess"),
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
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound") + " " +
            ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }

    public async Task<Response<IEnumerable<Supplier>>> Search(string slug)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                IQueryable<Supplier> query = _context.Suppliers;

                if (!string.IsNullOrEmpty(slug))
                {
                    query = query.Where(e => e.Slug.Contains(slug)
                                || e.Name.Contains(slug));
                }

                await transaction.CommitAsync();

                return new Response<IEnumerable<Supplier>>()
                {
                    Data = await query.ToListAsync(),
                    IsSuccess = true,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "GetSupplierSuccess"),
                    HttpStatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response<IEnumerable<Supplier>>()
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound") + " " +
            ex.Message.ToString(),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }
}