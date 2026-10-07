# Especificación de Característica: Módulo de Aprobación de Presupuestos y Gastos

**Rama de Características**: `003-budget-approval`

**Creado**: 2026-10-06

**Estado**: Borrador

**Entrada**: Descripción del usuario: "Módulo de Aprobación de Presupuestos y Gastos"

## Escenarios de Usuario y Pruebas *(obligatorio)*

### Historia de Usuario 1 - Solicitud de Presupuesto y Gastos (Prioridad: P1)

Como empleado o gerente de proyecto, quiero registrar solicitudes de presupuesto y gastos asociados a un proyecto (indicando monto, categoría, justificación y archivos adjuntos) para obtener financiamiento o reembolso.

**Por qué esta prioridad**: Permite el control financiero y la trazabilidad de los costos operativos por proyecto.

**Prueba Independiente**: Se puede probar creando una nueva solicitud de gasto con monto y justificación, y verificando que aparezca en el listado con estado "Pendiente".

**Escenarios de Aceptación**:

1. **Dado** que un usuario crea una solicitud de gasto válida, **Cuando** adjunta la justificación y envía el formulario, **Then** el sistema registra la solicitud y notifica al gerente de proyecto o aprobador correspondiente.
2. **Dado** que un usuario intenta enviar un gasto con monto negativo o cero, **When** intenta guardar, **Then** el sistema muestra un mensaje de error de validación.

---

### Historia de Usuario 2 - Aprobación o Rechazo de Gastos (Prioridad: P2)

Como gerente de proyecto o administrador, quiero revisar las solicitudes de presupuesto pendientes, aprobarlas o rechazarlas con observaciones para controlar el presupuesto asignado.

**Por qué esta prioridad**: Garantiza la supervisión y gobernanza financiera de la organización.

**Prueba Independiente**: Se puede probar accediendo al panel de aprobaciones, seleccionando una solicitud pendiente y cambiando su estado a "Aprobado" o "Rechazado".

**Escenarios de Aceptación**:

1. **Dado** que un aprobador revisa una solicitud pendiente, **When** hace clic en "Aprobar", **Then** el estado de la solicitud cambia a "Aprobado" y se descuenta del presupuesto disponible del proyecto.
2. **Dado** que un aprobador rechaza una solicitud con comentarios, **When** confirma el rechazo, **Then** el estado cambia a "Rechazado" y se notifica al solicitante.

---

### Historia de Usuario 3 - Reporte de Presupuesto Ejecutado (Prioridad: P3)

Como administrador o gerente, quiero visualizar un resumen gráfico y tabular del presupuesto total, gastado y disponible por proyecto para evitar sobrecostos.

**Por qué esta prioridad**: Proporciona visibilidad financiera en tiempo real.

**Prueba Independiente**: Se puede probar consultando la vista de presupuesto en los detalles del proyecto y verificando los cálculos de saldo disponible.

**Escenarios de Aceptación**:

1. **Dado** que un proyecto tiene gastos aprobados, **When** el usuario visualiza el resumen financiero, **Then** el sistema muestra el presupuesto total, el monto ejecutado y el saldo restante con indicadores visuales de alerta si se supera el 80%.

---

## Target Users & Permissions

- **Employees (Empleados)**: Crear solicitudes de presupuesto y gastos para proyectos en los que participan.
- **Project Managers (Gerentes de Proyecto)**: Aprobar o rechazar solicitudes de presupuesto para sus proyectos y gestionar sus asignaciones.
- **Administrators (Administradores)**: Acceso y control total de todas las solicitudes, auditoría y presupuestos corporativos.

### Requisitos Funcionales

- **FR-001**: El sistema DEBE permitir a los usuarios crear solicitudes de presupuesto y gastos asociadas a un proyecto.
- **FR-002**: El sistema DEBE validar montos máximos y requerir justificación obligatoria para cada gasto.
- **FR-003**: El sistema DEBE permitir a los Gerentes de Proyecto y Administradores aprobar o rechazar solicitudes.
- **FR-004**: El sistema DEBE actualizar automáticamente el presupuesto disponible del proyecto al aprobar o rechazar gastos.
- **FR-005**: El sistema DEBE generar notificaciones internas en la aplicación cuando una solicitud cambie de estado.

### Entidades Clave

- **BudgetRequest**: Solicitud de gasto (Id, ProjectId, UserId, Amount, Category, Justification, Status [Pending, Approved, Rejected], RequestDate, ApproverComments).
- **ProjectBudget**: Presupuesto asignado al proyecto (ProjectId, TotalBudget, AllocatedBudget).

## Criterios de Éxito *(obligatorio)*

### Resultados Medibles

- **SC-001**: Las solicitudes de presupuesto se registran y procesan en menos de 1 segundo.
- **SC-002**: El cálculo del presupuesto disponible es 100% preciso tras cada aprobación o rechazo.

## Supuestos

- Se integra con la base de datos SQLite y los modelos de Proyectos existentes en ContosoDashboard.
