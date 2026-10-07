# Tareas: Sistema de Reportes y Exportación PDF/Excel

**Entrada**: Documentos de diseño en `/specs/002-reports-export/` (`spec.md`, `plan.md`, `data-model.md`).

**Organización**: Las tareas están agrupadas por fases y historias de usuario (US1, US2, US3) para permitir la implementación y prueba independiente.

## Formato: `[ID] [P?] [Historia] Descripción`

- **[P]**: Puede ejecutarse en paralelo (diferentes archivos, sin dependencias previas).
- **[Historia]**: A qué historia de usuario pertenece (US1, US2, US3).

---

## Fase 1: Infraestructura y Modelos de Auditoría (Prerrequisitos)

- [x] T001 [P] Crear el modelo de datos `ExportLog` en `ContosoDashboard/Models/ExportLog.cs` con sus atributos (Id, UserId, ReportType, Format, ExportDate, Details).
- [x] T002 Actualizar el contexto de base de datos `ContosoDashboard/Data/ApplicationDbContext.cs` para incluir `DbSet<ExportLog> ExportLogs`.

---

## Fase 2: Servicios de Reportes y Analítica (US1)

- [x] T003 Crear la interfaz `IReportService` en `ContosoDashboard/Services/IReportService.cs` para definir métodos de obtención de estadísticas de proyectos, tareas, tipos de documentos más cargados, usuarios activos y patrones de acceso.
- [x] T004 Implementar `ReportService` en `ContosoDashboard/Services/ReportService.cs` utilizando Entity Framework Core contra la base de datos SQLite.
- [x] T005 Registrar `IReportService` como `ReportService` en el contenedor DI en `ContosoDashboard/Program.cs`.

---

## Fase 3: Exportación a Excel y PDF (US2, US3)

- [x] T006 [US2] Crear el servicio de exportación a Excel en `ContosoDashboard/Services/ExcelExportService.cs` para generar archivos `.xlsx` tabulares estructurados.
- [x] T007 [US3] Crear el servicio de exportación a PDF en `ContosoDashboard/Services/PdfExportService.cs` para generar reportes ejecutivos estilizados.
- [x] T008 [US2, US3] Crear el controlador API `ExportController` en `ContosoDashboard/Controllers/ExportController.cs` con endpoints seguros `/api/reports/export` que soporten formatos Excel y PDF, registrando cada acción en `ExportLog`.

---

## Fase 4: Interfaz de Usuario de Reportes (US1, US2, US3)

- [x] T009 [US1, US2, US3] Crear la página de visualización de reportes y auditoría en `ContosoDashboard/Pages/Reports.razor` con filtros por fecha, proyecto y botones de exportación a Excel/PDF.
- [x] T010 Actualizar el menú de navegación en `ContosoDashboard/Shared/NavMenu.razor` para incluir un enlace a la página de Reportes.

---

## Fase 5: Validación y Cierre

- [x] T011 Realizar pruebas de integración para la generación de reportes y descarga de archivos Excel/PDF.
- [x] T012 Verificar compilación limpia y ausencia de errores en .NET 8 / SQLite.
