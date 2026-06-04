---
type: entity
module: licenciamiento
layer: domain
status: implemented
source:
  - src/Debales.Domain/Licensing/License.cs
  - src/Debales.Domain/Licensing/LicenseModule.cs
  - src/Debales.Domain/Licensing/LicenseStatus.cs
related:
  - Licenciamiento
  - LicenseModule
  - SubscriptionPlan
---

# License (Licencia)

## Tabla EF / DbSet

`Licenses` — `DbSet<License>`
`LicenseModules` — `DbSet<LicenseModule>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `InstallationId` | `string` | Identificador único de la instalación (Machine Name en UI) |
| `PlanId` | `Guid` | FK a SubscriptionPlan |
| `LicenseeCompany` | `string` | Empresa titular |
| `LicenseeEmail` | `string` | Email de contacto (lowercase) |
| `LicenseKey` | `string` | Clave de licencia (uppercase) |
| `Status` | `LicenseStatus` | Estado actual |
| `StartsAt` | `DateTime` | Inicio de vigencia |
| `ExpiresAt` | `DateTime` | Expiración |
| `ActivatedAt` | `DateTime?` | Fecha de activación |
| `Notes` | `string?` | Notas de suspensión |

## Estados (LicenseStatus)

`Trial → Active | Expired | Suspended`

## LicenseModule — propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `LicenseId` | `Guid` | FK a License |
| `ModuleCode` | `string` | Código del módulo (uppercase) |
| `GrantedAt` | `DateTime` | Fecha de concesión |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(installationId, planId, company, email, key, startsAt, expiresAt, createdBy)` | Factory con validación |
| `Activate(updatedBy)` | Activa la licencia (no si Expired o Suspended) |
| `Suspend(notes, updatedBy)` | Suspende con notas |
| `AddModule(moduleCode, grantedBy)` | Añade módulo (idempotente) |
| `IsValid()` | `(Active|Trial) && StartsAt <= Now <= ExpiresAt` |
| `HasModule(code)` | `IsValid() && módulo en lista` |
| `CheckAndExpire()` | Marca Expired si fecha superada |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| SubscriptionPlan (FK PlanId) | Explícita |
| LicenseModule (colección) | Explícita 1:N |
