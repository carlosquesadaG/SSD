# Tareas: Módulo de Aprobación de Presupuestos y Gastos

**Entrada**: Documentos de diseño en `/specs/003-budget-approval/` (`spec.md`, `plan.md`, `data-model.md`).

## Formato: `[ID] [P?] [Historia] Descripción`

- **[P]**: Puede ejecutarse en paralelo.
- **[Historia]**: Historia de usuario (US1, US2, US3).

---

## Fase 1: Infraestructura y Modelos (Prerrequisitos)

- [x] T001 [P] Crear el modelo `BudgetRequest` en `ContosoDashboard/Models/BudgetRequest.cs` y añadir propiedades de presupuesto (`TotalBudget`, `SpentBudget`) en `Project.cs`.
- [x] T002 Actualizar `ApplicationDbContext.cs` para incluir `DbSet<BudgetRequest> BudgetRequests`.

---

## Fase 2: Lógica de Negocio y Servicio (US1, US2, US3)

- [x] T003 Crear la interfaz `IBudgetService` y `BudgetService` en `ContosoDashboard/Services/BudgetService.cs` para manejar creación, aprobación, rechazo y cálculo de saldos.
- [x] T004 Registrar `IBudgetService` en `Program.cs`.

---

## Fase 3: Interfaz de Usuario y Notificaciones (US1, US2, US3)

- [x] T005 Crear la página de gestión y solicitudes de presupuesto en `ContosoDashboard/Pages/Budgets.razor` con formularios de solicitud y paneles de aprobación.
- [x] T006 Actualizar el menú de navegación en `NavMenu.razor` para incluir la opción "Presupuestos".

---

## Fase 4: Validación y Cierre

- [x] T007 Verificar compilación limpia y ausencia de errores en .NET 8 / SQLite.
