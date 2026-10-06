# Modelo de Datos: Carga y Gestión de Documentos

**Rama de Características**: `001-document-upload` | **Fecha**: 2026-10-05

## Entidades Principales

### 1. `Document` (Documento)
Representa un archivo digital cargado en el sistema por un usuario, asociado opcionalmente a un proyecto y clasificado por categoría.

| Propiedad | Tipo | Restricciones / Atributos | Descripción |
|---|---|---|---|
| `Id` | `int` | Primary Key, Auto-increment | Identificador único del documento. |
| `Title` | `string` | Required, MaxLength(200) | Título descriptivo del documento. |
| `Description` | `string` | Optional, MaxLength(1000) | Descripción detallada del contenido o propósito. |
| `Category` | `string` | Required, MaxLength(100) | Categoría ("Project Documents", "Team Resources", "Personal Files", "Reports", "Presentations", "Other"). |
| `FilePath` | `string` | Required, MaxLength(500) | Ruta relativa o absoluta de almacenamiento en disco (fuera de `wwwroot`). |
| `FileName` | `string` | Required, MaxLength(255) | Nombre original del archivo proporcionado por el usuario. |
| `FileSize` | `long` | Required | Tamaño del archivo en bytes (máximo 25 MB / 26,214,400 bytes). |
| `FileType` | `string` | Required, MaxLength(255) | Tipo MIME del archivo (ej. `application/pdf`). |
| `UploadDate` | `DateTime` | Required | Fecha y hora UTC en que se cargó el archivo. |
| `UserId` | `string` | Required, MaxLength(450) | Identificador del usuario creador/propietario. |
| `ProjectId` | `int?` | Foreign Key (Project), Nullable | Proyecto asociado al documento (si aplica). |
| `Tags` | `string` | Optional, MaxLength(500) | Etiquetas de búsqueda separadas por comas. |

### 2. Relaciones con Entidades Existentes
- **Project**: Un proyecto (`Project`) puede tener múltiples documentos asociados (`1-to-Many`).
- **User**: Un usuario (`User` / identidad) puede cargar múltiples documentos (`1-to-Many`).

## Servicio de Almacenamiento (`IFileStorageService`)

```csharp
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string userId, int? projectId);
    Task<Stream> GetFileStreamAsync(string filePath);
    Task DeleteFileAsync(string filePath);
}
```
