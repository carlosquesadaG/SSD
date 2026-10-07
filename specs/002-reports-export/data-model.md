# Modelo de Datos: Sistema de Reportes y Exportación

**Rama de Características**: `002-reports-export` | **Fecha**: 2026-10-06

## Entidades Principales

### 1. `ExportLog` (Registro de Exportación)
Registra las acciones de exportación realizadas por los usuarios para fines de auditoría y métricas.

| Propiedad | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `int` | Primary Key, Auto-increment | Identificador único del registro. |
| `UserId` | `string` | Required, MaxLength(450) | ID del usuario que realizó la exportación. |
| `ReportType` | `string` | Required, MaxLength(100) | Tipo de reporte exportado ("Projects", "Tasks", "Documents"). |
| `Format` | `string` | Required, MaxLength(10) | Formato de archivo ("Excel", "PDF"). |
| `ExportDate` | `DateTime` | Required | Fecha y hora UTC de la exportación. |
| `Details` | `string` | Optional, MaxLength(500) | Detalles de filtros o parámetros aplicados. |
