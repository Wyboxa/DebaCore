---
type: audit
module: cross
layer: cross
status: not_confirmed
related:
  - 00 - Inicio
  - Licenciamiento
  - Despliegue
  - Pendientes priorizados
---

# Contradicciones detectadas entre CLAUDE.md y código real

## Contradicción 1 — Licenciamiento (CRÍTICA)

**CLAUDE.md dice** (§30, §46, §49.3):
> "Fase 6 — Licenciamiento — SIGUIENTE"  
> "Licensing | Pendiente — Fase 6 | —"

**Código real confirma**:
- `src/Debales.Domain/Licensing/License.cs` — entidad completa con lógica de dominio
- `src/Debales.Domain/Licensing/LicenseModule.cs` — entidad completa
- `src/Debales.Domain/Licensing/SubscriptionPlan.cs` — entidad completa
- `src/Debales.Application/Licensing/` — handlers: `GetCurrentLicenseHandler`, `GetSubscriptionPlansHandler`, `ActivateLicenseHandler`
- `src/Debales.Infrastructure/Persistence/Repositories/Licensing/` — repositorios
- `src/Debales.Infrastructure/Services/LicenseService.cs` — servicio
- `src/Debales.Infrastructure/Persistence/Migrations/20260604121322_AddLicensingModule.cs` — migración aplicada
- `src/Debales.Api/Controllers/LicensesController.cs` — controller con endpoints
- `src/Debales.Api/Controllers/SubscriptionPlansController.cs` — controller
- `src/Debales.Web/Components/Pages/Licencia/Licencia.razor` — UI completa con activación
- `src/Debales.Infrastructure/DependencyInjection.cs` — registros de DI: `ILicenseRepository`, `ISubscriptionPlanRepository`, `ILicenseService`

**Conclusión**: Licensing está completamente implementado (Dominio + Application + Infrastructure + API + UI). CLAUDE.md lo marca como pendiente.

**Acción recomendada**: Actualizar CLAUDE.md §46 y §49.3 para mover Licensing a "completo".

---

## Contradicción 2 — Docker Compose / Despliegue (CRÍTICA)

**CLAUDE.md dice** (§30):
> "Fase 7 — Despliegue local — [objetivo futuro]"

**Código real confirma**:
- `d:/Debales/docker-compose.yml` — compose con 3 servicios: `sqlserver`, `api`, `web`
- `d:/Debales/Dockerfile.api` — Dockerfile de la API
- `d:/Debales/Dockerfile.web` — Dockerfile de la UI
- El compose incluye healthcheck de SQL Server, dependencias entre servicios, volúmenes persistentes, variables de entorno por env

**Conclusión**: La infraestructura de Docker Compose está implementada. CLAUDE.md no lo refleja.

**Acción recomendada**: Actualizar CLAUDE.md §30 y §6 para reflejar que Fase 7 (Docker) está implementada.

---

## Contradicción 3 — Estado de migraciones

**CLAUDE.md dice** (§6):
> "9 migraciones aplicadas"

**Código real confirma**:
- 10 migraciones: la 10ª es `20260604121322_AddLicensingModule` (correspondiente a Licensing)

**Acción recomendada**: Actualizar §6 de CLAUDE.md de "9 migraciones" a "10 migraciones".

---

## Contradicción 4 — Estado de módulo AI en §49.3

**CLAUDE.md dice** (§49.3):
> "AI supervisada ERP (chat, anomalías, resúmenes) | Completo — ERP-6 | sin migración propia"

**Código real confirma**: Correcto. No hay contradicción aquí — esta parte está bien documentada.

---

## Contradicción 5 — §18 Fuera de alcance "Facturación legal completa"

**CLAUDE.md dice** (§18):
> "Facturación legal completa — Fuera de alcance inicial"

**Código real**: Hay un módulo de facturación completo con facturas, rectificativas, vencimientos, cobros y pagos. Aunque no llega al nivel de "facturación legal completa" (sin PDF, sin firma, sin SII), el alcance real supera significativamente lo descrito en §18.

**Conclusión**: Ambigüedad de definición, no contradicción crítica. §18 usa el término "completa" para referirse a funcionalidad avanzada (firma digital, SII, etc.).

---

## Resumen

| Contradicción | Severidad | Acción |
|---------------|-----------|--------|
| Licensing marcado como pendiente | Alta | Actualizar CLAUDE.md §46 y §49.3 |
| Docker no documentado | Alta | Actualizar CLAUDE.md §30 y §6 |
| "9 migraciones" incorrecto | Media | Actualizar CLAUDE.md §6 |
| §18 "facturación fuera de alcance" | Baja | Clarificar qué se entiende por "completa" |
