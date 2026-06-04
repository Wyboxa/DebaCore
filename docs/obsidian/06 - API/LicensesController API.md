---
type: api
module: licenciamiento
layer: api
status: implemented
source:
  - src/Debales.Api/Controllers/LicensesController.cs
  - src/Debales.Api/Controllers/SubscriptionPlansController.cs
related:
  - Licenciamiento
  - License
  - SubscriptionPlan
---

# LicensesController

**Ruta base**: `api/licenses` (LicensesController) + `api/subscription-plans` (SubscriptionPlansController)  
**Autorización**: JWT Bearer requerido

## Endpoints

| Método | Ruta | Handler | Descripción |
|--------|------|---------|-------------|
| GET | `/api/licenses/current` | `GetCurrentLicenseHandler` | Licencia activa actual (null si no hay) |
| POST | `/api/licenses/activate` | `ActivateLicenseHandler` | Activar licencia con clave y plan |
| GET | `/api/subscription-plans` | `GetSubscriptionPlansHandler` | Lista planes disponibles |

## Request body (POST activate)

```csharp
record ActivateLicenseRequest(
    string LicenseKey,
    string InstallationId,
    string LicenseeCompany,
    string LicenseeEmail,
    string PlanCode,
    DateTime StartsAt,
    DateTime ExpiresAt,
    IReadOnlyList<string> ModuleCodes)
```

## Notas

- `GET /current` devuelve 404 si no hay licencia activa
- La clave se almacena en uppercase
- `ModuleCodes` puede ser lista vacía `[]`
