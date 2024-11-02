
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Form;
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
                    Imgae = sup.Imgae,
                    CreatedAt = DateTime.UtcNow,
                    CategoryId = sup.CategoryId,
                    ProductId = sup.ProductId,
                };

                await _context.Suppliers.AddAsync(supplier);
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
                supplier.Imgae = sup.Imgae;
                supplier.UpdatedAt = DateTime.UtcNow;
                supplier.CategoryId = sup.CategoryId;
                supplier.ProductId = sup.ProductId;

                _context.Suppliers.Update(supplier);
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
                                            Imgae = sup.Imgae,
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            CategoryId = sup.CategoryId,
                                            ProductId = sup.ProductId,
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
                                            Imgae = sup.Imgae,
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            CategoryId = sup.CategoryId,
                                            ProductId = sup.ProductId,
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
    // public async Task<List<Dictionary<string, object>>> GetExportData(DateTime? start, DateTime? end, List<string> selectedFields)
    // {
    //     try
    //     {
    //         var query = from sup in _context.Suppliers
    //                     where (!start.HasValue || sup.created_at >= start.Value)
    //                     && (!end.HasValue || sup.created_at <= end.Value)
    //                     select new
    //                     {
    //                         sup,
    //                         Category = sup.category
    //                     };

    //         var suppliers = await query.ToListAsync();

    //         var result = new List<Dictionary<string, object>>();

    //         foreach (var item in suppliers)
    //         {
    //             var dict = new Dictionary<string, object>();

    //             var supplier = item.sup;

    //             // Add only the selected fields to the dictionary
    //             if (selectedFields.Contains("id")) dict["id"] = supplier.id;
    //             if (selectedFields.Contains("name")) dict["name"] = supplier.name;
    //             if (selectedFields.Contains("product")) dict["product"] = supplier.product;
    //             if (selectedFields.Contains("price")) dict["price"] = supplier.price;
    //             if (selectedFields.Contains("slug")) dict["slug"] = supplier.slug;
    //             if (selectedFields.Contains("contact")) dict["contact"] = supplier.contact;
    //             if (selectedFields.Contains("isTaking")) dict["isTaking"] = supplier.isTalking;
    //             if (selectedFields.Contains("email")) dict["email"] = supplier.email;
    //             if (selectedFields.Contains("active")) dict["active"] = supplier.active;
    //             if (selectedFields.Contains("photoUrl")) dict["photoUrl"] = supplier.photoUrl;
    //             if (selectedFields.Contains("created_at")) dict["created_at"] = supplier.created_at;
    //             if (selectedFields.Contains("updated_at")) dict["updated_at"] = supplier.updated_at;

    //             if (selectedFields.Contains("category") && supplier.category != null)
    //             {
    //                 var categoryData = new Dictionary<string, object>();
    //                 if (selectedFields.Contains("category_id")) categoryData["category_Id"] = supplier.category.Id;
    //                 // if (selectedFields.Contains("Category_Title")) categoryData["Category_Title"] = supplier.category.Category_Title;

    //                 dict["category"] = categoryData;
    //             }

    //             result.Add(dict);
    //         }

    //         return result;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.ToString());
    //     }
    // }

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
                                            Imgae = sup.Imgae,
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            CategoryId = sup.CategoryId,
                                            ProductId = sup.ProductId,
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

    // public async Task<IActionResult> GetForm()
    // {
    //     var formConfig = new FormConfig
    //     {
    //         Title = "Supplier",
    //         Layout = "horizontal",
    //         LabelCol = 6,
    //         WrapperCol = 18,
    //         FormItem = Forms.GetFormFields()
    //     };
    //     var result = new
    //     {
    //         form = formConfig
    //     };

    //     return await Task.FromResult(new OkObjectResult(result));
    // }
}