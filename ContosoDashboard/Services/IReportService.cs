using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoDashboard.Models;

namespace ContosoDashboard.Services
{
    public class ReportSummaryDto
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int TotalDocuments { get; set; }
        public List<DocumentTypeCountDto> DocumentsByType { get; set; } = new();
        public List<UserActivityDto> TopUploaders { get; set; } = new();
    }

    public class DocumentTypeCountDto
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class UserActivityDto
    {
        public string UserId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int UploadCount { get; set; }
    }

    public interface IReportService
    {
        Task<ReportSummaryDto> GetReportSummaryAsync(int? projectId = null, DateTime? startDate = null, DateTime? endDate = null);
        Task LogExportAsync(string userId, string reportType, string format, string? details);
    }
}
