
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

                var categories = await _context.Categories
                .Where(p => sup.Categories.Select(sp => sp.Id).Contains(p.Id))
                .ToListAsync();

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
                    Categories = categories,
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
                supplier.Image = sup.Image;
                supplier.UpdatedAt = DateTime.UtcNow;
                supplier.Product = sup.Product;
                supplier.Categories = sup.Categories;

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
                                .Include(c => c.Categories)
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
                                    Categories = sup.Categories,
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
                                .Include(c => c.Categories)
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
                                    Categories = sup.Categories,
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
                                .Include(c => c.Categories)
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
                                    Categories = sup.Categories,
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
}