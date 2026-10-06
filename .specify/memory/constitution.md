# ContosoDashboard Project Constitution

## Core Principles

### I. Clean Architecture & Modularity
- Services must be decoupled using clear interfaces (e.g., `IFileStorageService`, `IDashboardService`) to support pluggable implementations (such as local storage transitioning to Azure Blob Storage).
- Separation of concerns between UI (Razor Components/Pages), Services, and Data Access (Entity Framework Core).

### II. Security & Robust File Handling
- Files must be stored securely outside of web-accessible roots (`wwwroot`) with authorization checks enforced on all download/access endpoints.
- Strict validation of file types (whitelist) and file size limits (e.g., maximum 25 MB).
- Prevention of path traversal and duplicate key violations through unique GUID-based paths generated prior to persistence.

### III. Robust Testing & Quality Assurance
- Automated unit tests and integration tests must validate service layers, database contexts, and core business workflows.
- Strict adherence to error handling and validation workflows with user-friendly error and success notifications.

### IV. Observability & Maintainability
- Structured logging required across all services and database operations.
- Clear code documentation and strong typing (`Nullable` enabled, strict null checks) across the .NET 8 codebase.

## Technology Stack Constraints
- **Framework**: .NET 8 (ASP.NET Core Blazor Server / MVC / Entity Framework Core).
- **Database**: SQL Server with Entity Framework Core migrations.
- **Authentication**: Microsoft Identity Web / OpenID Connect.

## Development Workflow & Governance
- All pull requests and changes must preserve existing compilation stability, maintain robust authorization checks, and respect architectural abstractions.
- Complexity must be justified; follow YAGNI (You Aren't Gonna Need It) and KISS (Keep It Simple, Stupid) principles.

**Version**: 1.0.0 | **Ratified**: 2026-10-05 | **Last Amended**: 2026-10-05
