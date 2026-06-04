---
type: module
module: licenciamiento
layer: cross
status: implemented
source:
  - src/Debales.Domain/Licensing/
  - src/Debales.Application/Licensing/
  - src/Debales.Infrastructure/Persistence/Repositories/Licensing/
  - src/Debales.Infrastructure/Services/LicenseService.cs
  - src/Debales.Api/Controllers/LicensesController.cs
  - src/Debales.Api/Controllers/SubscriptionPlansController.cs
  - src/Debales.Web/Components/Pages/Licencia/Licencia.razor
related:
  - License
  - LicenseModule
  - SubscriptionPlan
---

# Módulo Licenciamiento

## AVISO: Contradicción con CLAUDE.md

CLAUDE.md §46 y §49.3 declaran que "Fase 6 — Licenciamiento" está pendiente. El código confirma que está completamente implementado con migración aplicada (`AddLicensingModule`, 2026-06-04). Ver [[Contradicciones detectadas]].

## Qué problema resuelve

Control de licencias de la plataforma: planes de suscripción, activación, módulos contratados, validación de vigencia y estados de licencia.

## Estado

**Implementado** — migración `AddLicensingModule` (2026-06-04).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[License]] | Licencia con clave, empresa, plan, fechas y módulos contratados |
| [[LicenseModule]] | Módulo específico concedido en una licencia |
| [[SubscriptionPlan]] | Plan de suscripción con límites de usuarios, módulos y flag IA |

## Reglas de dominio

- `ExpiresAt > StartsAt` — validado en `License.Create()`
- `License.IsValid()` = Status Active o Trial + fechas vigentes
- `License.HasModule(code)` = IsValid + módulo en lista
- Activar licencia Expired o Suspended lanza excepción
- `CheckAndExpire()` — marca como Expired si fecha superada

## Estados de licencia

`Trial → Active | Expired | Suspended`

## Handlers

| Handler | Tipo | Descripción |
|---------|------|-------------|
| `GetCurrentLicenseHandler` | Query | Obtiene licencia vigente actual |
| `GetSubscriptionPlansHandler` | Query | Lista planes disponibles |
| `ActivateLicenseHandler` | Command | Activa una licencia con clave, plan y módulos |

## Controllers

| Controller | Ruta | Endpoints |
|------------|------|-----------|
| `LicensesController` | `api/licenses` | GET `/current`, POST `/activate` |
| `SubscriptionPlansController` | `api/subscription-plans` | GET / |

## Página Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Licencia.razor` | `/licencia` | Implementada — muestra estado, plan, módulos y formulario de activación |

## Repositorios

- `ILicenseRepository` → `LicenseRepository`
- `ISubscriptionPlanRepository` → `SubscriptionPlanRepository`

## Servicios

- `ILicenseService` → `LicenseService` — lógica de validación y verificación de expiración

## Plans sembrados (inferido — SubscriptionPlan.ForSeed existe)

Los planes se definen con `ForSeed` factory method. Los datos exactos no se leen en el seeder principal, pero la migración puede incluir data seeds.

## Lo que está completo

- Entidades de dominio completas
- Handlers de activación y consulta
- UI de licencia con visualización de estado, plan y módulos
- Formulario de activación en UI
- Validación de vigencia

## Lo que falta

- Validación de licencia en middleware (bloqueo gradual si caduca)
- Modo offline con expiración controlada
- Renovación desde UI
- CLAUDE.md debe actualizarse para reflejar que está implementado
