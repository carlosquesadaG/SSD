# Tareas: Carga y Gestión de Documentos

**Entrada**: Documentos de diseño actualizados en `/specs/001-document-upload/` (`spec.md`, `plan.md`, `data-model.md`).

**Organización**: Las tareas están agrupadas por fases y funcionalidades para permitir la implementación y prueba independiente.

## Formato: `[ID] [P?] [Historia] Descripción`

- **[P]**: Puede ejecutarse en paralelo (diferentes archivos, sin dependencias previas).
- **[Historia]**: A qué historia de usuario pertenece (US1, US2, US3).

---

## Fase 1: Infraestructura Compartida (Prerrequisitos)

- [x] T001 [P] Crear el modelo de datos `Document` en `ContosoDashboard/Models/Document.cs` con sus atributos (Id, Title, Description, Category, FilePath, FileName, FileSize, FileType, UploadDate, UserId, ProjectId, Tags).
- [x] T002 Actualizar el contexto de base de datos `ContosoDashboard/Data/ApplicationDbContext.cs` para incluir `DbSet<Document> Documents`.
- [x] T003 Crear la interfaz `IFileStorageService` en `ContosoDashboard/Services/IFileStorageService.cs` con métodos para guardar, descargar, previsualizar y eliminar archivos.
- [x] T004 Implementar `LocalFileStorageService` en `ContosoDashboard/Services/LocalFileStorageService.cs` utilizando rutas seguras en `AppData/uploads/{userId}/{projectId or "personal"}/{guid}.{extension}` fuera de `wwwroot`.
- [x] T005 Registrar `IFileStorageService` como `LocalFileStorageService` en el contenedor DI en `ContosoDashboard/Program.cs`.

---

## Fase 2: Carga y Almacenamiento Seguro (US1)

- [x] T006 [US1] Crear el servicio de lógica de negocio para documentos `DocumentService` en `ContosoDashboard/Services/DocumentService.cs` para manejar validaciones de tamaño (25 MB), tipos permitidos y persistencia de metadatos.
- [x] T007 [US1] Crear el componente Razor de carga de documentos en `ContosoDashboard/Pages/DocumentUploadComponent.razor` con selector de archivos, campos de metadatos (Título, Categoría, Proyecto, Etiquetas) e indicador de progreso.
- [x] T008 [US1] Implementar validación en el lado del servidor y cliente para asegurar que se rechacen archivos >25 MB o tipos no soportados con mensajes de error claros.

---

## Fase 3: Exploración, Filtrado y Organización (US2)

- [x] T009 [US2] Añadir métodos de consulta en `DocumentService` para obtener documentos por usuario ("Mis Documentos") y por proyecto con soporte de filtros, búsqueda por texto y ordenamiento.
- [x] T010 [US2] Crear la página de visualización de documentos del usuario en `ContosoDashboard/Pages/Documents.razor` con opciones de filtrado, búsqueda y ordenamiento.
- [x] T011 [US2] Actualizar la página `ContosoDashboard/Pages/ProjectDetails.razor` para incluir la sección de Documentos del Proyecto.
- [x] T012 [US2] Implementar el widget de "Documentos Recientes" en la página de inicio/Dashboard (`ContosoDashboard/Pages/Index.razor`).

---

## Fase 4: Descarga Segura, Previsualización y Control de Acceso (US3)

- [x] T013 [US3] Crear el controlador API `DocumentController` en `ContosoDashboard/Controllers/DocumentController.cs` con endpoints de descarga `/api/documents/{id}/download` y previsualización en navegador para PDF e imágenes.
- [x] T014 [US3] Implementar la verificación de autorización en el controlador para asegurar que solo usuarios autorizados (propietarios, miembros del proyecto o administradores) puedan acceder al archivo.
- [x] T015 [US3] Añadir funcionalidades de edición de metadatos, reemplazo de archivos y eliminación de documentos en `DocumentService` y la interfaz de usuario.
- [x] T016 [US3] Integrar la asociación de documentos directamente desde la vista de detalles de tareas (`ContosoDashboard/Pages/Tasks.razor` o similar).

---

## Fase 5: Validación y Cierre

- [x] T017 Realizar pruebas de extremo a extremo (E2E) para el flujo completo: carga -> previsualización -> descarga segura -> widgets de dashboard.
- [x] T018 Verificar compilación limpia y ausencia de errores en .NET 8.
