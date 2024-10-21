
using System.Net;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Entities;
using Server.Helper;
using Server.Utilities.Response;

namespace Server.Services;

public class ReportService : IReport
{
    private EFDataContext _context;
    private readonly ApplicationSettings _appSettings;
    public ReportService(EFDataContext context, ApplicationSettings applicationSettings)
    {
        _context = context;
        _appSettings = applicationSettings;
    }

    public async Task<List<Report>> GetAll()
    {
        try
        {
            var report = await (from rep in _context.Reports
                                select new Report
                                {
                                    Inventory_Id = rep.Inventory_Id,
                                    Product_Id = rep.Product_Id,
                                    User_Id = rep.User_Id,
                                    QuantityOnHand = rep.QuantityOnHand,
                                    Created_At = rep.Created_At,
                                    Updated_At = rep.Updated_At,
                                }).OrderByDescending(x => x.Report_Id).ToListAsync();
            return report;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<Report> GetById(Guid ReportId)
    {
        try
        {
            var report = await (from rep in _context.Reports
                                where rep.Report_Id == ReportId
                                select new Report
                                {
                                    Inventory_Id = rep.Inventory_Id,
                                    Product_Id = rep.Product_Id,
                                    User_Id = rep.User_Id,
                                    QuantityOnHand = rep.QuantityOnHand,
                                    Created_At = rep.Created_At,
                                    Updated_At = rep.Updated_At,
                                }).FirstOrDefaultAsync();
            return report;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public async Task<Response<Report>> Add(Report rep)
    {
        try
        {
            var report = new Report()
            {
                Inventory_Id = rep.Inventory_Id,
                Product_Id = rep.Product_Id,
                User_Id = rep.User_Id,
                QuantityOnHand = rep.QuantityOnHand,
                Created_At = rep.Created_At,
                Updated_At = rep.Updated_At,
            };

            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();

            return new Response<Report>
            {
                ReportId = report.Report_Id,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("ReportMessages", "CreateReportSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the Report: {ex.Message}");
        }
    }

    public async Task<Response<Report>> Delete(Guid ReportId)
    {
        try
        {
            if (ReportId <= Guid.Empty)
            {
                return new Response<Report>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("ReportMessages", "ReportNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            var report = _context.Reports.Where(x => x.Report_Id == ReportId).FirstOrDefault();
            if (report == null)
            {
                return new Response<Report>
                {
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("ReportMessages", "ReportNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return new Response<Report>
            {
                IsSuccess = true,
                ReportId = ReportId,
                Message = _appSettings.GetConfigurationValue("ReportMessages", "DeleteReportSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Report>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("ReportMessages", "DeleteReportFailure") + " " +
            ex.Message.ToString(),
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<Response<Report>> Update(Report rep, Guid ReportId)
    {
        try
        {
            if (rep.Report_Id <= Guid.Empty)
            {
                return new Response<Report>
                {
                    ReportId = ReportId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("ReportMessages", "ReportNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            var report = await _context.Reports.FirstOrDefaultAsync(x => x.Report_Id == ReportId);
            if (report == null)
            {
                return new Response<Report>
                {
                    ReportId = ReportId,
                    IsSuccess = false,
                    Message = _appSettings.GetConfigurationValue("ReportMessages", "ReportNotFound"),
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Update properties
            report.Inventory_Id = rep.Inventory_Id;
            report.Product_Id = rep.Product_Id;
            report.User_Id = rep.User_Id;
            report.QuantityOnHand = rep.QuantityOnHand;
            report.Created_At = rep.Created_At;
            report.Updated_At = rep.Updated_At;

            _context.Reports.Update(report);
            await _context.SaveChangesAsync();

            return new Response<Report>
            {
                ReportId = ReportId,
                IsSuccess = true,
                Message = _appSettings.GetConfigurationValue("ReportMessages", "UpdateReportSuccess"),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new Response<Report>
            {
                IsSuccess = false,
                Message = _appSettings.GetConfigurationValue("ReportMessages", "UpdateReportFailure") + ": " + ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}