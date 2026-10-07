using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoDashboard.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportSummaryDto> GetReportSummaryAsync(int? projectId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var projectsQuery = _context.Projects.AsQueryable();
            var tasksQuery = _context.Tasks.AsQueryable();
            var docsQuery = _context.Documents.AsQueryable();

            if (projectId.HasValue)
            {
                projectsQuery = projectsQuery.Where(p => p.ProjectId == projectId.Value);
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
                docsQuery = docsQuery.Where(d => d.ProjectId == projectId.Value);
            }

            if (startDate.HasValue)
            {
                docsQuery = docsQuery.Where(d => d.UploadDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                docsQuery = docsQuery.Where(d => d.UploadDate <= endDate.Value);
            }

            var summary = new ReportSummaryDto
            {
                TotalProjects = await projectsQuery.CountAsync(),
                ActiveProjects = await projectsQuery.CountAsync(p => p.Status == ProjectStatus.Active),
                TotalTasks = await tasksQuery.CountAsync(),
                CompletedTasks = await tasksQuery.CountAsync(t => t.Status == Models.TaskStatus.Completed),
                OverdueTasks = await tasksQuery.CountAsync(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != Models.TaskStatus.Completed),
                TotalDocuments = await docsQuery.CountAsync()
            };

            summary.DocumentsByType = await docsQuery
                .GroupBy(d => d.Category)
                .Select(g => new DocumentTypeCountDto { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var topUploaderIds = await docsQuery
                .GroupBy(d => d.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            summary.TopUploaders = new List<UserActivityDto>();
            foreach (var item in topUploaderIds)
            {
                var user = await _context.Users.FindAsync(int.TryParse(item.UserId, out int uid) ? uid : 0);
                summary.TopUploaders.Add(new UserActivityDto
                {
                    UserId = item.UserId,
                    DisplayName = user?.DisplayName ?? $"Usuario {item.UserId}",
                    UploadCount = item.Count
                });
            }

            return summary;
        }

        public async Task LogExportAsync(string userId, string reportType, string format, string? details)
        {
            var log = new ExportLog
            {
                UserId = userId,
                ReportType = reportType,
                Format = format,
                ExportDate = DateTime.UtcNow,
                Details = details
            };

            _context.ExportLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
