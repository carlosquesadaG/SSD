using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContosoDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContosoDashboard.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ExportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportReport([FromQuery] string format = "excel", [FromQuery] int? projectId = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "1";
            var summary = await _reportService.GetReportSummaryAsync(projectId);

            if (format.Equals("pdf", StringComparison.OrdinalIgnoreCase))
            {
                await _reportService.LogExportAsync(userId, "ProjectSummary", "PDF", $"ProjectId: {projectId}");
                
                // Generate simple formatted HTML/PDF text or stream
                var sb = new StringBuilder();
                sb.AppendLine("========================================");
                sb.AppendLine(" CONTOSO DASHBOARD - REPORTE EJECUTIVO");
                sb.AppendLine("========================================");
                sb.AppendLine($"Fecha de generación: {DateTime.UtcNow:g}");
                sb.AppendLine($"Total Proyectos: {summary.TotalProjects} (Activos: {summary.ActiveProjects})");
                sb.AppendLine($"Total Tareas: {summary.TotalTasks} (Completadas: {summary.CompletedTasks}, Vencidas: {summary.OverdueTasks})");
                sb.AppendLine($"Total Documentos: {summary.TotalDocuments}");
                sb.AppendLine("----------------------------------------");
                sb.AppendLine("Documentos por Categoría:");
                foreach (var cat in summary.DocumentsByType)
                {
                    sb.AppendLine($" - {cat.Category}: {cat.Count}");
                }
                sb.AppendLine("========================================");

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/plain", $"ReporteEjecutivo_{DateTime.UtcNow:yyyyMMdd}.txt");
            }
            else
            {
                await _reportService.LogExportAsync(userId, "ProjectSummary", "Excel", $"ProjectId: {projectId}");

                // Generate CSV/Excel compatible tabular format
                var sb = new StringBuilder();
                sb.AppendLine("Metrica,Valor");
                sb.AppendLine($"Total Proyectos,{summary.TotalProjects}");
                sb.AppendLine($"Proyectos Activos,{summary.ActiveProjects}");
                sb.AppendLine($"Total Tareas,{summary.TotalTasks}");
                sb.AppendLine($"Tareas Completadas,{summary.CompletedTasks}");
                sb.AppendLine($"Tareas Vencidas,{summary.OverdueTasks}");
                sb.AppendLine($"Total Documentos,{summary.TotalDocuments}");

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", $"ReporteProyectos_{DateTime.UtcNow:yyyyMMdd}.csv");
            }
        }
    }
}
