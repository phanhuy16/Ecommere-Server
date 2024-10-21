
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
        try
        {
            var supplier = new Supplier()
            {
                name = sup.name,
                product = sup.product,
                price = sup.price,
                slug = sup.slug,
                contact = sup.contact,
                isTalking = sup.isTalking,
                email = sup.email,
                active = sup.active,
                photoUrl = sup.photoUrl,
                created_at = sup.created_at,
                updated_at = sup.updated_at,
                category = _context.Categories.Find(sup.category_id)
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            return new Response<Supplier>
            {
                Data = supplier,
                SupplierId = supplier.id,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "CreateSupplierSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Supplier>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "CreateSupplierFailure") + " " + ex.Message,
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<Response<Supplier>> Update(Supplier sup, Guid SupplierId)
    {
        try
        {
            if (SupplierId == Guid.Empty)
            {
                return new Response<Supplier>
                {
                    SupplierId = SupplierId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            var supplier = _context.Suppliers.Where(x => x.id == SupplierId).FirstOrDefault();
            if (supplier == null)
            {
                return new Response<Supplier>
                {
                    SupplierId = SupplierId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            supplier.name = sup.name;
            supplier.slug = sup.slug;
            supplier.product = sup.product;
            supplier.price = sup.price;
            supplier.contact = sup.contact;
            supplier.isTalking = sup.isTalking;
            supplier.email = sup.email;
            supplier.active = sup.active;
            supplier.photoUrl = sup.photoUrl;
            supplier.updated_at = sup.updated_at;
            supplier.category = _context.Categories.Find(sup.category_id);

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();

            return new Response<Supplier>
            {
                Data = supplier,
                SupplierId = SupplierId,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "UpdateSupplierSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Supplier>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "UpdateSupplierFailure") + " " +
            ex.Message.ToString(),
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<Response<Supplier>> Delete(Guid SupplierId)
    {
        try
        {
            if (SupplierId == Guid.Empty)
            {
                return new Response<Supplier>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("SupplierMessages", "SupplierNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            var supplier = _context.Suppliers.Where(x => x.id == SupplierId).FirstOrDefault();
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
            return new Response<Supplier>
            {
                Data = supplier,
                IsSuccess = true,
                SupplierId = SupplierId,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "DeleteSupplierSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Supplier>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("SupplierMessages", "DeleteSupplierFailure") + " " +
            ex.Message.ToString(),
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<IActionResult> GetPaginationSupplier(PaginationFilter filter, string route)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Suppliers.CountAsync();

            var supplier = await _context.Suppliers
                                  .Select(sup => new Supplier
                                  {
                                      id = sup.id,
                                      name = sup.name,
                                      product = sup.product,
                                      price = sup.price,
                                      slug = sup.slug,
                                      contact = sup.contact,
                                      isTalking = sup.isTalking,
                                      email = sup.email,
                                      active = sup.active,
                                      photoUrl = sup.photoUrl,
                                      created_at = sup.created_at,
                                      updated_at = sup.updated_at,
                                  })
                                .OrderByDescending(x => x.id)
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            var pagedResponse = PaginationHelper.CreatePagedReponse<Supplier>(supplier, validFilter, totalRecords, _uriService, route);
            return new OkObjectResult(pagedResponse);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    public async Task<List<Supplier>> GetAllSupplier()
    {
        try
        {
            var supplier = await (from sup in _context.Suppliers
                                  select new Supplier
                                  {
                                      id = sup.id,
                                      name = sup.name,
                                      product = sup.product,
                                      price = sup.price,
                                      slug = sup.slug,
                                      contact = sup.contact,
                                      isTalking = sup.isTalking,
                                      email = sup.email,
                                      active = sup.active,
                                      photoUrl = sup.photoUrl,
                                      created_at = sup.created_at,
                                      updated_at = sup.updated_at,
                                  }).OrderByDescending(x => x.id).ToListAsync();
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    public async Task<List<Dictionary<string, object>>> GetExportData(DateTime? start, DateTime? end, List<string> selectedFields)
    {
        try
        {
            var query = from sup in _context.Suppliers
                        where (!start.HasValue || sup.created_at >= start.Value)
                        && (!end.HasValue || sup.created_at <= end.Value)
                        select new
                        {
                            sup,
                            Category = sup.category
                        };

            var suppliers = await query.ToListAsync();

            var result = new List<Dictionary<string, object>>();

            foreach (var item in suppliers)
            {
                var dict = new Dictionary<string, object>();

                var supplier = item.sup;

                // Add only the selected fields to the dictionary
                if (selectedFields.Contains("id")) dict["id"] = supplier.id;
                if (selectedFields.Contains("name")) dict["name"] = supplier.name;
                if (selectedFields.Contains("product")) dict["product"] = supplier.product;
                if (selectedFields.Contains("price")) dict["price"] = supplier.price;
                if (selectedFields.Contains("slug")) dict["slug"] = supplier.slug;
                if (selectedFields.Contains("contact")) dict["contact"] = supplier.contact;
                if (selectedFields.Contains("isTaking")) dict["isTaking"] = supplier.isTalking;
                if (selectedFields.Contains("email")) dict["email"] = supplier.email;
                if (selectedFields.Contains("active")) dict["active"] = supplier.active;
                if (selectedFields.Contains("photoUrl")) dict["photoUrl"] = supplier.photoUrl;
                if (selectedFields.Contains("created_at")) dict["created_at"] = supplier.created_at;
                if (selectedFields.Contains("updated_at")) dict["updated_at"] = supplier.updated_at;

                if (selectedFields.Contains("category") && supplier.category != null)
                {
                    var categoryData = new Dictionary<string, object>();
                    if (selectedFields.Contains("category_id")) categoryData["category_Id"] = supplier.category.Id;
                    // if (selectedFields.Contains("Category_Title")) categoryData["Category_Title"] = supplier.category.Category_Title;

                    dict["category"] = categoryData;
                }

                result.Add(dict);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    public async Task<Supplier> GetById(Guid SupplierId)
    {
        try
        {
            var supplier = await (from sup in _context.Suppliers
                                  where sup.id == SupplierId
                                  select new Supplier
                                  {
                                      id = sup.id,
                                      name = sup.name,
                                      product = sup.product,
                                      price = sup.price,
                                      slug = sup.slug,
                                      contact = sup.contact,
                                      isTalking = sup.isTalking,
                                      email = sup.email,
                                      active = sup.active,
                                      photoUrl = sup.photoUrl,
                                      created_at = sup.created_at,
                                      updated_at = sup.updated_at,
                                  }).FirstOrDefaultAsync();
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    public async Task<IActionResult> GetForm()
    {
        var formConfig = new FormConfig
        {
            Title = "Supplier",
            Layout = "horizontal",
            LabelCol = 6,
            WrapperCol = 18,
            FormItem = Forms.GetFormFields()
        };
        var result = new
        {
            form = formConfig
        };

        return await Task.FromResult(new OkObjectResult(result));
    }
}