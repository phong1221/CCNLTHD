using Backend.DTO;
using Backend.DTO.ReportDTO;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }
        [HttpPost]
        public IActionResult createReport([FromBody] ReportRequest reportRequest)
        {
            var result = reportService.createReport(reportRequest);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult getAllReport()
        {
            var result = reportService.GetAllReports();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult getReport(int id)
        {
            var report = reportService.GetReport(id);
            return Ok(report);
        }
        [HttpGet("user/{userId}")]
        public IActionResult getReportByUser(int userId)
        {
            var result = reportService.GetReportByUser(userId);
            return Ok(result);
        }
        [HttpGet("post/{postId}")]
        public IActionResult getReportByPost(int postId)
        {
            var result = reportService.GetReportByPost(postId);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult deleteReport(int id)
        {
            reportService.delete(id);
            return Ok("xoa thanh cong");
        }
        [HttpPut("{id}")]
        public IActionResult updateReport(int id, [FromBody] ReportRequest reportRequest)
        {
            var result = reportService.updateReport(id, reportRequest);
            return Ok(result);
        }
        [HttpPut("accept/{id}")]
        public IActionResult acceptReportAndHidePost(int id)
        {
            reportService.AcceptReportAndHidePost(id);
            return Ok("report da duoc chap thuan");
        }
        [HttpGet("page")]
        public IActionResult getPage([FromQuery] PageRequest pageRequest)
        {
            var result = reportService.GetPage(pageRequest.page, pageRequest.pageSize);
            return Ok(result);
        }
    }
}
