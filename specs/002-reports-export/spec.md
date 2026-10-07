# Especificación de Característica: Sistema de Reportes y Exportación PDF/Excel

**Rama de Características**: `002-reports-export`

**Creado**: 2026-10-06

**Estado**: Borrador

**Entrada**: Descripción del usuario: "Sistema de Reportes y Exportación PDF/Excel"

## Escenarios de Usuario y Pruebas *(obligatorio)*

### Historia de Usuario 1 - Generación y Visualización de Reportes de Proyectos y Tareas (Prioridad: P1)

Como gerente de proyecto o administrador, quiero generar reportes resumidos del estado de proyectos y tareas (progreso, tareas vencidas, carga de trabajo del equipo) para evaluar el desempeño y tomar decisiones informadas.

**Por qué esta prioridad**: Es fundamental para la supervisión directiva y el seguimiento del rendimiento de la corporación.

**Prueba Independiente**: Se puede probar accediendo a la nueva página de Reportes, seleccionando un rango de fechas y un proyecto, y verificando que las métricas y tablas de resumen se muestren correctamente.

**Escenarios de Aceptación**:

1. **Dado** que un gerente accede a la sección de Reportes, **Cuando** selecciona la opción de reporte de proyectos activos y tareas pendientes, **Then** el sistema muestra un resumen consolidado con gráficos y tablas detalladas.
2. **Dado** que se filtra por un rango de fechas sin actividad, **When** se genera el reporte, **Then** el sistema muestra un mensaje indicando que no hay datos para el periodo seleccionado.

---

### Historia de Usuario 2 - Exportación de Reportes a Formato Excel (Prioridad: P2)

Como usuario autorizado, quiero exportar los datos de reportes y listados a un archivo Excel (.xlsx) para realizar análisis avanzados y auditorías offline.

**Por qué esta prioridad**: Facilita la interoperabilidad de datos con herramientas de hoja de cálculo estándar de la industria.

**Prueba Independiente**: Se puede probar haciendo clic en el botón "Exportar a Excel" en la vista de reportes o documentos y verificando la descarga del archivo `.xlsx`.

**Escenarios de Aceptación**:

1. **Dado** que el usuario visualiza un reporte en pantalla, **When** hace clic en el botón de exportación a Excel, **Then** el navegador descarga un archivo `.xlsx` estructurado con los datos correspondientes.

---

### Historia de Usuario 3 - Exportación de Reportes a Formato PDF (Prioridad: P3)

Como usuario autorizado, quiero exportar reportes ejecutivos en formato PDF formateado profesionalmente para compartirlos con directivos y stakeholders externos.

**Por qué esta prioridad**: Permite la distribución de informes ejecutivos listos para impresión o presentación formal.

**Prueba Independiente**: Se puede probar haciendo clic en el botón "Exportar a PDF" y verificando la recepción del documento PDF formateado.

**Escenarios de Aceptación**:

1. **Dado** que el usuario visualiza un reporte ejecutivo, **When** hace clic en el botón de exportación a PDF, **Then** el sistema genera y descarga un documento PDF estilizado con el logotipo de Contoso y la información resumida.

---

## Requisitos *(obligatorio)*

### Requisitos Funcionales

- **FR-001**: El sistema DEBE proporcionar una vista centralizada de "Reportes y Auditoría" accesible para roles de Administradores y Gerentes de Proyecto.
- **FR-002**: El sistema DEBE permitir generar reportes de seguimiento de actividad de documentos incluyendo: tipos de documentos más cargados, usuarios con mayor actividad de carga y patrones de acceso a documentos.
- **FR-003**: El sistema DEBE permitir filtrar los reportes por proyecto, rango de fechas y tipo de actividad.
- **FR-004**: El sistema DEBE permitir la exportación de datos tabulares a formato Excel (`.xlsx`) y PDF (`.pdf`) formateados profesionalmente.
- **FR-005**: El sistema DEBE registrar todas las acciones de exportación y auditoría en `ExportLog`.

### Entidades Clave

- **ReportDefinition**: Define los parámetros y filtros para la generación de reportes.
- **ExportLog**: Registro de auditoría de archivos exportados (usuario, fecha, tipo de reporte, formato).

## Criterios de Éxito *(obligatorio)*

### Resultados Medibles

- **SC-001**: Los reportes se generan en menos de 2 segundos para conjuntos de datos menores a 1,000 registros.
- **SC-002**: Los archivos Excel y PDF se descargan correctamente con 100% de precisión en los datos tabulares.

## Supuestos

- Se utilizará una librería ligera compatible con .NET 8 (como EPPlus o ClosedXML para Excel, y QuestPDF o iTextSharp para PDF) para la generación de archivos.
