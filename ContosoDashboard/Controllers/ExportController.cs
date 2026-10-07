using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContosoDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

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
                
                var htmlContent = $@"<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <title>Reporte Ejecutivo - Contoso Corporation</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #333; margin: 40px; }}
        .header {{ border-bottom: 3px solid #005a9e; padding-bottom: 20px; margin-bottom: 30px; display: flex; justify-content: space-between; align-items: center; }}
        .logo {{ font-size: 24px; font-weight: bold; color: #005a9e; }}
        .meta {{ font-size: 14px; color: #666; text-align: right; }}
        h1 {{ font-size: 22px; color: #111; margin-bottom: 5px; }}
        .metrics-grid {{ display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; margin-bottom: 30px; }}
        .card {{ background: #f4f6f8; border-radius: 8px; padding: 20px; border-left: 5px solid #005a9e; }}
        .card h3 {{ margin: 0 0 10px 0; font-size: 14px; color: #555; text-transform: uppercase; }}
        .card p {{ margin: 0; font-size: 28px; font-weight: bold; color: #111; }}
        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
        th, td {{ padding: 12px 15px; border-bottom: 1px solid #ddd; text-align: left; }}
        th {{ background-color: #005a9e; color: white; font-weight: 600; }}
        tr:nth-child(even) {{ background-color: #f9f9f9; }}
        .footer {{ margin-top: 50px; text-align: center; font-size: 12px; color: #888; border-top: 1px solid #ddd; padding-top: 15px; }}
        @media print {{
            body {{ margin: 15px; }}
            .no-print {{ display: none; }}
        }}
    </style>
</head>
<body>
    <div class=""no-print"" style=""background: #fff3cd; padding: 10px 15px; margin-bottom: 20px; border-radius: 5px; border: 1px solid #ffeeba; text-align: center;"">
        💡 <strong>Consejo para guardar como PDF:</strong> Presione <kbd>Ctrl + P</kbd> (o <kbd>Cmd + P</kbd> en Mac) y seleccione <em>""Guardar como PDF""</em> en el destino de impresión.
    </div>

    <div class=""header"">
        <div>
            <div class=""logo"">CONTOSO CORPORATION</div>
            <h1>Reporte Ejecutivo de Proyectos y Tareas</h1>
        </div>
        <div class=""meta"">
            <strong>Fecha:</strong> {DateTime.UtcNow:dd/MM/yyyy HH:mm} UTC<br>
            <strong>Generado por:</strong> Usuario ID {userId}
        </div>
    </div>

    <div class=""metrics-grid"">
        <div class=""card"">
            <h3>Proyectos Activos / Total</h3>
            <p>{summary.ActiveProjects} / {summary.TotalProjects}</p>
        </div>
        <div class=""card"" style=""border-left-color: #28a745;"">
            <h3>Tareas Completadas</h3>
            <p>{summary.CompletedTasks} / {summary.TotalTasks}</p>
        </div>
        <div class=""card"" style=""border-left-color: #dc3545;"">
            <h3>Tareas Vencidas</h3>
            <p>{summary.OverdueTasks}</p>
        </div>
    </div>

    <h2>Desglose de Documentos por Categoría</h2>
    <table>
        <thead>
            <tr>
                <th>Categoría de Documento</th>
                <th>Cantidad</th>
            </tr>
        </thead>
        <tbody>
            " + (summary.DocumentsByType.Count == 0 ? "<tr><td colspan='2'>No hay documentos registrados.</td></tr>" : string.Join("", summary.DocumentsByType.Select(d => $"<tr><td>{d.Category}</td><td>{d.Count}</td></tr>"))) + @"
        </tbody>
    </table>

    <h2 style=""margin-top: 40px;"">Usuarios con Mayor Actividad</h2>
    <table>
        <thead>
            <tr>
                <th>Usuario</th>
                <th>Documentos Cargados</th>
            </tr>
        </thead>
        <tbody>
            " + (summary.TopUploaders.Count == 0 ? "<tr><td colspan='2'>Sin actividad registrada.</td></tr>" : string.Join("", summary.TopUploaders.Select(u => $"<tr><td>{u.DisplayName}</td><td>{u.UploadCount}</td></tr>"))) + @"
        </tbody>
    </table>

    <div class=""footer"">
        ContosoDashboard Enterprise &copy; {DateTime.UtcNow.Year} - Todos los derechos reservados.
    </div>
</body>
</html>";

                var bytes = Encoding.UTF8.GetBytes(htmlContent);
                return File(bytes, "text/html", $"ReporteEjecutivo_{DateTime.UtcNow:yyyyMMdd}.html");
            }
            else
            {
                await _reportService.LogExportAsync(userId, "ProjectSummary", "Excel", $"ProjectId: {projectId}");

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
