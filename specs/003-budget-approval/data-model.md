# Modelo de Datos: Módulo de Presupuestos y Gastos

**Rama de Características**: `003-budget-approval` | **Fecha**: 2026-10-06

## Entidades Principales

### 1. `BudgetRequest` (Solicitud de Presupuesto / Gasto)
Representa una solicitud de gasto monetario asociada a un proyecto.

| Propiedad | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `int` | Primary Key, Auto-increment | Identificador único de la solicitud. |
| `ProjectId` | `int` | Foreign Key (Project), Required | Proyecto al que se imputa el gasto. |
| `UserId` | `string` | Required, MaxLength(450) | Empleado que solicita el gasto. |
| `Amount` | `decimal` | Required, Range(0.01, 1000000) | Monto solicitado. |
| `Category` | `string` | Required, MaxLength(100) | Categoría del gasto (Viajes, Software, Hardware, Servicios, Otros). |
| `Justification` | `string` | Required, MaxLength(1000) | Justificación detallada del gasto. |
| `Status` | `string` | Required, Default: "Pending" | Estado de la solicitud ("Pending", "Approved", "Rejected"). |
| `RequestDate` | `DateTime` | Required | Fecha y hora UTC de la solicitud. |
| `ApproverComments` | `string` | Optional, MaxLength(500) | Comentarios del aprobador. |
