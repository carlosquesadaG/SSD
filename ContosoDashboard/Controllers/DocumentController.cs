using System.IO;
using System.Threading.Tasks;
using ContosoDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContosoDashboard.Controllers
{
    [Route("api/documents")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IFileStorageService _fileStorageService;

        public DocumentController(IDocumentService documentService, IFileStorageService fileStorageService)
        {
            _documentService = documentService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Administrator");
            if (document.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            try
            {
                var stream = await _fileStorageService.GetFileStreamAsync(document.FilePath);
                return File(stream, document.FileType, document.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("El archivo físico no se encuentra en el servidor.");
            }
        }

        [HttpGet("{id}/preview")]
        public async Task<IActionResult> Preview(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();

            try
            {
                var stream = await _fileStorageService.GetFileStreamAsync(document.FilePath);
                return File(stream, document.FileType);
            }
            catch (FileNotFoundException)
            {
                return NotFound("El archivo físico no se encuentra en el servidor.");
            }
        }
    }
}
