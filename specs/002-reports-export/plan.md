# Plan de Implementación: Sistema de Reportes y Exportación PDF/Excel

**Rama de Características**: `002-reports-export` | **Fecha**: 2026-10-06 | **Especificación**: [spec.md](./spec.md)

**Entrada**: Especificación de característica en [`specs/002-reports-export/spec.md`](./spec.md).

## Resumen

Implementación de un módulo completo de reportes analíticos y exportación a formatos Excel (`.xlsx`) y PDF (`.pdf`) para ContosoDashboard (.NET 8 Blazor / Entity Framework Core / SQLite). Permite a los usuarios generar resúmenes estadísticos de proyectos, tareas y documentos, y descargarlos en formatos profesionales.

## Contexto Técnico

- **Lenguaje/Versión**: C# / .NET 8
- **Dependencias Principales**: ASP.NET Core Blazor Server, Entity Framework Core, System.IO.Compression / bibliotecas nativas de exportación de datos tabulares (CSV/Excel liviano o EPPlus/ClosedXML).
- **Almacenamiento**: SQLite (base de datos existente) + Entidad de auditoría `ExportLog`.
- **Pruebas**: Pruebas unitarias e integración para servicios de reportes y exportación.
- **Plataforma Objetivo**: Servidor Windows / Linux con .NET 8 Runtime.
- **Tipo de Proyecto**: Aplicación Web ASP.NET Core Blazor Server.
- **Objetivos de Rendimiento**: Generación de reportes y exportaciones en menos de 2 segundos.
- **Restricciones**: Cumplimiento de políticas de seguridad, autorización estricta para Gerentes de Proyecto y Administradores.

## Verificación de Constitución

- [x] **Arquitectura Limpia y Modular**: Uso de interfaces (`IReportService`, `IExportService`) separando la lógica analítica de la interfaz de usuario Blazor.
- [x] **Seguridad y Control de Acceso**: Endpoints y páginas protegidos por políticas de autorización.
- [x] **Pruebas y Calidad**: Cobertura de pruebas para la generación de reportes y registros de exportación.
- [x] **Stack Tecnológico**: .NET 8, EF Core, SQLite.

## Estructura del Proyecto

### Documentación (esta característica)

```text
specs/002-reports-export/
├── plan.md              # Este archivo
├── data-model.md        # Definición de entidades de auditoría
└── tasks.md             # Tareas detalladas
```

### Código Fuente (raíz del repositorio)

```text
ContosoDashboard/
├── Models/
│   └── ExportLog.cs
├── Services/
│   ├── IReportService.cs
│   ├── ReportService.cs
│   ├── IExportService.cs
│   └── ExportService.cs
├── Controllers/
│   └── ExportController.cs
├── Pages/
│   └── Reports.razor
└── Data/
    └── ApplicationDbContext.cs (actualizado con DbSet<ExportLog>)
```

**Decisión de Estructura**: Se integra dentro de la arquitectura modular existente de ContosoDashboard, añadiendo servicios dedicados para analítica y exportación.
