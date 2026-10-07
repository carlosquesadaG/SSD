using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoDashboard.Models
{
    public class ExportLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ReportType { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Format { get; set; } = string.Empty; // "Excel" or "PDF"

        [Required]
        public DateTime ExportDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Details { get; set; }
    }
}
