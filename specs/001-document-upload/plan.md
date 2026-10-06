# Plan de Implementación: Carga y Gestión de Documentos

**Rama de Características**: `001-document-upload` | **Fecha**: 2026-10-05 | **Especificación**: [spec.md](./spec.md)

**Entrada**: Especificación de característica en [`specs/001-document-upload/spec.md`](./spec.md).

## Resumen

Implementación de un sistema completo de carga, organización, previsualización, búsqueda y descarga segura de documentos en ContosoDashboard (.NET 8 Blazor / Entity Framework Core / SQL Server). Los archivos se almacenarán de forma segura fuera de `wwwroot` (`AppData/uploads`) utilizando rutas basadas en GUID únicos con la abstracción `IFileStorageService`. Incluye integración con tareas, widgets en el Dashboard y notificaciones.

## Contexto Técnico

- **Lenguaje/Versión**: C# / .NET 8
- **Dependencias Principales**: ASP.NET Core Blazor Server, Entity Framework Core 8 (SQL Server), Microsoft Identity / Authentication.
- **Almacenamiento**: Base de datos SQL Server (metadatos) + Sistema de archivos local (`AppData/uploads`) con abstracción `IFileStorageService` para futura migración a Azure Blob Storage.
- **Pruebas**: xUnit / Moq / Pruebas de integración de servicios.
- **Plataforma Objetivo**: Servidor Windows / Linux con .NET 8 Runtime.
- **Tipo de Proyecto**: Aplicación Web ASP.NET Core MVC & Blazor Server.
- **Objetivos de Rendimiento**: Carga de archivos en <3s, filtrado y búsqueda en <500ms.
- **Restricciones**: Tamaño máximo por archivo de 25 MB; almacenamiento fuera de `wwwroot` con endpoints de controlador protegidos por autorización.

## Verificación de Constitución

- [x] **Arquitectura Limpia y Modular**: Uso de interfaces (`IFileStorageService`) y separación de responsabilidades entre UI, Servicios y Acceso a Datos.
- [x] **Seguridad y Manejo Robusto de Archivos**: Almacenamiento fuera de `wwwroot`, comprobaciones de autorización en descargas, nombres basados en GUID para prevenir path traversal.
- [x] **Pruebas y Calidad**: Cobertura de pruebas unitarias e integración para servicios de archivos y metadatos.
- [x] **Stack Tecnológico**: .NET 8, EF Core 8, SQL Server.

## Estructura del Proyecto

### Documentación (esta característica)

```text
specs/001-document-upload/
├── plan.md              # Este archivo
├── data-model.md        # Definición de entidades y modelo de datos
└── tasks.md             # Tareas detalladas (generadas por /speckit.tasks)
```

### Código Fuente (raíz del repositorio)

```text
ContosoDashboard/
├── Models/
│   ├── Document.cs
│   ├── DocumentCategory.cs
│   └── ...
├── Services/
│   ├── IFileStorageService.cs
│   ├── LocalFileStorageService.cs
│   └── DocumentService.cs
├── Controllers/
│   └── DocumentController.cs
├── Pages/
│   ├── Documents.razor
│   └── ProjectDetails.razor (actualizado con sección de documentos)
└── Data/
    └── ApplicationDbContext.cs (actualizado con DbSet<Document>)
```

**Decisión de Estructura**: Se utiliza la estructura estándar de aplicación web ASP.NET Core Blazor de ContosoDashboard, añadiendo modelos, servicios, controladores de descarga y componentes Razor dedicados a la gestión documental.

## Modelo de Datos y Entidades

### 1. Entidad `Document`
- `Id` (int, PK)
- `Title` (string, requerido, máx 200 chars)
- `Description` (string, opcional, máx 1000 chars)
- `Category` (string, requerido: "Project Documents", "Team Resources", "Personal Files", "Reports", "Presentations", "Other")
- `FilePath` (string, requerido, ruta física en `AppData/uploads/...`)
- `FileName` (string, nombre original del archivo)
- `FileSize` (long, tamaño en bytes)
- `FileType` (string, MIME type, máx 255 chars)
- `UploadDate` (DateTime, fecha de carga)
- `UserId` (string, ID del usuario que cargó el archivo)
- `ProjectId` (int?, opcional, FK a `Projects`)
- `Tags` (string, etiquetas separadas por comas o JSON)

### 2. Patrón de Almacenamiento de Archivos (`IFileStorageService`)
- Ruta segura fuera de `wwwroot`: `AppData/uploads/{userId}/{projectId or "personal"}/{guid}.{extension}`
- Secuencia de carga: Generar ruta única -> Guardar archivo en disco -> Guardar metadatos en base de datos (evita huérfanos y duplicados).
