using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ContosoDashboard.Services
{
    public interface IDocumentService
    {
        Task<List<Document>> GetDocumentsByUserAsync(string userId);
        Task<List<Document>> GetDocumentsByProjectAsync(int projectId);
        Task<List<Document>> GetRecentDocumentsAsync(string userId, int count = 5);
        Task<Document?> GetDocumentByIdAsync(int id);
        Task<Document> UploadDocumentAsync(IFormFile file, string title, string? description, string category, string userId, int? projectId, string? tags);
        Task DeleteDocumentAsync(int id, string userId, bool isProjectManager);
        Task UpdateDocumentAsync(int id, string title, string? description, string category, string? tags, string userId);
    }

    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 25 * 1024 * 1024; // 25 MB

        public DocumentService(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<Document>> GetDocumentsByUserAsync(string userId)
        {
            return await _context.Documents
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        public async Task<List<Document>> GetDocumentsByProjectAsync(int projectId)
        {
            return await _context.Documents
                .Where(d => d.ProjectId == projectId)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        public async Task<List<Document>> GetRecentDocumentsAsync(string userId, int count = 5)
        {
            return await _context.Documents
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.UploadDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<Document> UploadDocumentAsync(IFormFile file, string title, string? description, string category, string userId, int? projectId, string? tags)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo está vacío.");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("El archivo excede el tamaño máximo permitido de 25 MB.");

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException($"El tipo de archivo '{extension}' no está permitido.");

            // Save file to secure storage before db record insertion
            string filePath = await _fileStorageService.SaveFileAsync(file, userId, projectId);

            var document = new Document
            {
                Title = title,
                Description = description,
                Category = category,
                FilePath = filePath,
                FileName = file.FileName,
                FileSize = file.Length,
                FileType = file.ContentType ?? "application/octet-stream",
                UploadDate = DateTime.UtcNow,
                UserId = userId,
                ProjectId = projectId,
                Tags = tags
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }

        public async Task DeleteDocumentAsync(int id, string userId, bool isProjectManager)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return;

            if (doc.UserId != userId && !isProjectManager)
            {
                throw new UnauthorizedAccessException("No tiene permisos para eliminar este documento.");
            }

            await _fileStorageService.DeleteFileAsync(doc.FilePath);
            _context.Documents.Remove(doc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDocumentAsync(int id, string title, string? description, string category, string? tags, string userId)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) throw new KeyNotFoundException("Documento no encontrado.");

            if (doc.UserId != userId)
                throw new UnauthorizedAccessException("Solo el propietario puede editar los metadatos de este documento.");

            doc.Title = title;
            doc.Description = description;
            doc.Category = category;
            doc.Tags = tags;

            await _context.SaveChangesAsync();
        }
    }
}
