using Backend.DTO;
using Backend.DTO.ReportDTO;

namespace Backend.Services.Interfaces
{
    public interface IReportService
    {
        ReportResponse createReport(ReportRequest reportRequest);
        ReportResponse updateReport(int id,ReportRequest reportRequest);
        ReportResponse GetReport(int id);
        List<ReportResponse> GetAllReports();
        List<ReportResponse> GetReportByUser(int UserId);
        List<ReportResponse> GetReportByPost(int PostId);
        void AcceptReportAndHidePost(int id);
        void delete(int id);
        PageResult<ReportResponse> GetPage(int page, int pageSize);
    }
}
