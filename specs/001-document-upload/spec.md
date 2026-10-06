# Especificación de Característica: Carga y Gestión de Documentos

**Rama de Características**: `001-document-upload`

**Creado**: 2026-10-05

**Estado**: Borrador

**Entrada**: Descripción del usuario: "Implementar capacidades de carga y gestión de documentos basadas en StakeholderDocs/document-upload-and-management-feature.md"

## Escenarios de Usuario y Pruebas *(obligatorio)*

### Historia de Usuario 1 - Carga y Almacenamiento Seguro de Documentos (Prioridad: P1)

Como empleado o gerente de proyectos, quiero cargar documentos relacionados con el trabajo (PDF, documentos de Office, texto e imágenes de hasta 25 MB) con metadatos (título, categoría, proyecto asociado y etiquetas personalizadas) para que los documentos se almacenen y organicen de forma segura.

**Por qué esta prioridad**: Funcionalidad principal necesaria para cualquier capacidad de gestión de documentos; permite el almacenamiento seguro fuera de `wwwroot` siguiendo patrones de almacenamiento offline.

**Prueba Independiente**: Se puede probar iniciando sesión, cargando un documento PDF válido con metadatos y verificando que el archivo se almacene en `AppData/uploads` y que los metadatos se registren en la base de datos.

**Escenarios de Aceptación**:

1. **Dado** que un usuario ha iniciado sesión en ContosoDashboard, **Cuando** selecciona un archivo PDF válido de menos de 25 MB, completa el título y la categoría, y envía la carga, **Then** el sistema carga el archivo con éxito, muestra un mensaje de éxito y lo lista en "Mis Documentos".
2. **Dado** que un usuario intenta cargar un archivo que supera los 25 MB o un tipo de archivo no compatible, **Cuando** envía la carga, **Then** el sistema rechaza el archivo y muestra un mensaje de error adecuado.

---

### Historia de Usuario 2 - Exploración, Filtrado y Organización de Documentos (Prioridad: P2)

Como usuario, quiero ver mis documentos cargados así como los documentos específicos de un proyecto, y filtrarlos u ordenarlos por categoría, fecha, título o proyecto para poder localizar fácilmente los archivos necesarios.

**Por qué esta prioridad**: Característica de usabilidad esencial que permite a los empleados y gerentes encontrar documentos rápidamente.

**Prueba Independiente**: Se puede probar navegando a las páginas de Mis Documentos y Detalles del Proyecto, aplicando filtros de categoría y opciones de ordenamiento, y verificando que aparezcan las listas filtradas correctas.

**Escenarios de Aceptación**:

1. **Dado** que existen múltiples documentos en diferentes categorías y proyectos, **When** el usuario aplica un filtro de categoría o los ordena por fecha de carga, **Then** la lista de documentos se actualiza inmediatamente para reflejar el criterio.
2. **Dado** que un usuario visualiza la página de Detalles del Proyecto, **When** revisa la sección de Documentos del Proyecto, **Then** se muestran todos los documentos asociados con dicho proyecto con opciones de descarga.

---

### Historia de Usuario 3 - Descarga Segura y Control de Acceso (Prioridad: P3)

Como empleado o administrador, quiero descargar documentos de forma segura con comprobaciones de autorización adecuadas para que los documentos confidenciales del proyecto estén protegidos contra accesos no autorizados.

**Por qué esta prioridad**: Garantiza el cumplimiento de la seguridad y el control de acceso adecuado (Empleados, Líderes de Equipo, Gerentes de Proyecto, Administradores).

**Prueba Independiente**: Se puede probar intentando descargar un documento de proyecto como miembro autorizado del equipo frente a un usuario no autorizado.

**Escenarios de Aceptación**:

1. **Dado** que un usuario autorizado hace clic en descargar en un documento, **When** la solicitud de descarga es procesada por el endpoint seguro del controlador, **Then** el archivo se transmite de forma segura al cliente.
2. **Dado** que un usuario no autorizado intenta acceder directamente a una URL de documento restringido, **When** fallan las comprobaciones de autorización, **Then** el sistema devuelve un error 403 Prohibido (Forbidden) o una respuesta de redirección.

---

## Requisitos *(obligatorio)*

### Requisitos Funcionales

- **FR-001**: El sistema DEBE permitir a los usuarios seleccionar y cargar uno o varios archivos (PDF, Word, Excel, PowerPoint, texto, JPEG, PNG).
- **FR-002**: El sistema DEBE aplicar un límite máximo de tamaño de archivo de 25 MB por archivo con mensajes de error claros en caso de infracción.
- **FR-003**: El sistema DEBE requerir un título de documento y la selección de una categoría (Documentos de Proyecto, Recursos de Equipo, Archivos Personales, Informes, Presentaciones, Otros) al momento de la carga.
- **FR-004**: El sistema DEBE permitir la asociación opcional con un proyecto y etiquetas personalizadas opcionales.
- **FR-005**: El sistema DEBE capturar automáticamente la fecha y hora de carga, el nombre del usuario que cargó el archivo, el tamaño del archivo y el tipo de archivo (tipo MIME de hasta 255 caracteres).
- **FR-006**: El sistema DEBE almacenar los archivos de forma segura fuera de `wwwroot` (por ejemplo, `AppData/uploads`) utilizando rutas basadas en GUID únicos (`{userId}/{projectId or "personal"}/{uniqueId}.{extension}`) generadas antes de la inserción en la base de datos.
- **FR-007**: El sistema DEBE proporcionar una vista de Mis Documentos que permita ordenar y filtrar por categoría, proyecto, rango de fechas y título.
- **FR-008**: El sistema DEBE proporcionar una vista de Documentos del Proyecto en las páginas de detalles del proyecto accesible para los miembros del mismo, permitiendo además adjuntar documentos a tareas y asociarlos automáticamente al proyecto correspondiente.
- **FR-009**: El sistema DEBE proporcionar endpoints seguros de controlador de descarga y previsualización en el navegador para tipos comunes (PDF, imágenes), aplicando comprobaciones estrictas de autorización.
- **FR-010**: El sistema DEBE definir una abstracción `IFileStorageService` que admita almacenamiento de archivos local con preparación futura para Azure Blob Storage.
- **FR-011**: El sistema DEBE permitir a los usuarios editar metadatos y reemplazar archivos de documentos que hayan subido, así como eliminarlos (con permisos para Project Managers en sus proyectos).
- **FR-012**: El sistema DEBE incluir integración con el panel principal (Dashboard widget de "Documentos Recientes") y notificaciones dentro de la aplicación ante eventos de documentos compartidos o nuevos documentos en proyectos.

### Entidades Clave

- **Document (Documento)**: Representa un archivo cargado con atributos (Id, Título, Descripción, Categoría, Ruta de Archivo, Tamaño de Archivo, Tipo de Archivo, Fecha de Carga, ID de Usuario, ID de Proyecto, Etiquetas).
- **DocumentCategory (Categoría de Documento)**: Categorías predefinidas para la organización de documentos.
- **DocumentTag (Etiqueta de Documento)**: Etiquetas personalizadas asociadas con documentos para mejorar la búsqueda.

## Criterios de Éxito *(obligatorio)*

### Resultados Medibles

- **SC-001**: Los usuarios pueden cargar con éxito un documento válido de menos de 25 MB en menos de 3 segundos.
- **SC-002**: El 100% de los archivos cargados se almacenan de forma segura fuera de `wwwroot` con sus metadatos correctamente vinculados en la base de datos.
- **SC-003**: Los usuarios no autorizados son bloqueados con éxito de descargar documentos restringidos de proyectos con un 100% de confiabilidad.
- **SC-004**: Los usuarios pueden filtrar y ordenar las listas de documentos en menos de 500 ms.

## Supuestos

- Los usuarios objetivo tienen sesiones autenticadas a través de Microsoft Identity u otro proveedor de autenticación personalizado.
- El almacenamiento de archivos local en `AppData/uploads` es suficiente para la implementación inicial, con `IFileStorageService` permitiendo una migración fluida a Azure Blob posteriormente.
- Las reglas de membresía del proyecto definen quién puede acceder a los documentos específicos de cada proyecto.
