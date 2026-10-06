using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoDashboard.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = "Other";

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }

        [Required]
        [StringLength(255)]
        public string FileType { get; set; } = string.Empty;

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        [StringLength(500)]
        public string? Tags { get; set; }
    }
}
