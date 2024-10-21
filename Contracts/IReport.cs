

using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;

public interface IReport
{
    Task<List<Report>> GetAll();
    Task<Report> GetById(Guid ReportId);
    Task<Response<Report>> Add(Report rep);
    Task<Response<Report>> Update(Report rep, Guid ReportId);
    Task<Response<Report>> Delete(Guid ReportId);
}