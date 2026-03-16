using Backend.Data;
using Backend.DTO;
using Backend.DTO.ReportDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Services
{
    public class ReportService : IReportService
    {
        private readonly BlogDbContext dbContext;
        private readonly IAuthService authService;

        public ReportService(BlogDbContext dbContext, IAuthService authService)
        {
            this.dbContext = dbContext;
            this.authService = authService;
        }

        public void AcceptReportAndHidePost(int id)
        {
            var report = dbContext.Reports.FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.IsAccept == false);
            if (report == null)
            {
                throw new Exception("Report khong ton tai hoac da duoc accept rui  ");
            }
            if (!authService.IsAdmin())
            {
                throw new Exception("chỉ có admin mới có quyền này");
            }
            report.IsAccept = true;
            var post = dbContext.Posts.FirstOrDefault(p => p.Id == report.PostId && p.IsDeleted ==false );
            if (post == null)
            {
                throw new Exception("post da bi xoa hoac khong ton tai");
            }
            post.IsDeleted=true;
            dbContext.SaveChanges();

        }

        public ReportResponse createReport(ReportRequest reportRequest)
        {
            var userId = authService.GetCurrentUser().Id;
            var post = dbContext.Posts.FirstOrDefault(p => p.Id == reportRequest.PostId);
            if (post == null)
            {
                throw new Exception("Post này không tồn tại ");
            }
            var report = new Report()
            {
                UserId = userId,
                PostId = reportRequest.PostId,
                Reason = reportRequest.Reason,
                CreatedAt = DateTime.Now,
                IsAccept = false ,
                IsDeleted = false ,
            };
            dbContext.Reports.Add(report);
            dbContext.SaveChanges();
            report.Post = post;

            return mapToResponse(report);
        }

        public List<ReportResponse> GetAllReports()
        { 
            var listReport = dbContext.Reports
                .Where(r => r.IsDeleted==false)
                .Include(r => r.User)
                .Include(r => r.Post)
                .ToList();
            var report = listReport
               .Select(r => mapToResponse(r))
               .ToList();
            return report;
        }

        public PageResult<ReportResponse> GetPage(int page, int pageSize)
        {
            var total = dbContext.Reports
                .Where(p => p.IsDeleted == false)
                .Count();
            var item = dbContext.Reports
                .Where (r => r.IsDeleted==false)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Include (r => r.User)
                .Include(r => r.Post)
                .ToList();
            var report = item 
                .Select(r => mapToResponse(r))
                .ToList();
            return new PageResult<ReportResponse>{
              Items = report ,
              Total = total
            };
        }

        public ReportResponse GetReport(int id)
        {
            var report = dbContext.Reports
                .Where(r => r.Id == id && r.IsDeleted == false)
                .Include((r) => r.User)
                .Include(r => r.Post)
                .FirstOrDefault();
            return mapToResponse(report);
        }

        public List<ReportResponse> GetReportByPost(int PostId)
        {
           var listReport= dbContext.Reports
                .Where(r => r.PostId == PostId && r.IsDeleted== false)
                .Include(r => r.User)
                .Include(r => r.Post)
                .ToList();
            var result = listReport
               .Select(r => mapToResponse(r))
               .ToList();
            return result ;
        }

        public List<ReportResponse> GetReportByUser(int UserId)
        {
            var listReport=dbContext.Reports
                .Where(r => r.UserId == UserId && r.IsDeleted==false)
                .Include(r =>r.User)
                .Include(r => r.Post)
                .ToList() ;
            var result =listReport
                .Select(r => mapToResponse(r))
                .ToList();
            return result ;
        }

        public ReportResponse updateReport(int id, ReportRequest reportRequest)
        {
            var check= dbContext.Reports.FirstOrDefault(r =>r.Id==id && r.IsDeleted == false && r.IsAccept == false);
            if (check == null)
            {
                throw new Exception("report da bi xoa hoac da duoc phe duyet");
            }
            if (!authService.IsAuthor(check.UserId))
            {
                throw new Exception("bạn không có quyền sửa báo cáo  này");
            }
            check.Reason = reportRequest.Reason;
            dbContext.SaveChanges();
            return mapToResponse(check);

        }

        public void delete(int id)
        {   
            var report = dbContext.Reports.FirstOrDefault(r => r.Id == id && r.IsDeleted==false && r.IsAccept == false);
            if(report == null)
            {
                throw new Exception("khong tim thay report hoac da duoc accept");
            }
            if (!authService.IsAuthor(report.UserId))
            {
                throw new Exception("bạn không có quyền xoá báo cáo  này");
            }
            report.IsDeleted = true;
            dbContext.SaveChanges();
        }
        public ReportResponse mapToResponse(Report report)
        {   if(report == null)
            {
                throw new Exception("Report khong ton tai");
            }
            var response = new ReportResponse()
            {
                Id = report.Id,
                UserId = report.UserId,
                PostId = report.PostId,
                Reason = report.Reason,
                CreatedAt = report.CreatedAt,
                IsAccept = report.IsAccept,
                IsDeleted = report.IsDeleted,
                PostTitle = report.Post?.Title,
                UserName = report.User?.UserName
            };
            return response;
        }
    }
}
