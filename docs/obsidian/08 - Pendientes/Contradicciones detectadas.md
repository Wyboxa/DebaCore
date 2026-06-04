---
type: audit
module: cross
layer: cross
status: resolved
related:
  - 00 - Inicio
  - Licenciamiento
  - Despliegue
  - Pendientes priorizados
---

# Contradicciones detectadas entre CLAUDE.md y código real

> **Última revisión: 2026-06-05** — Todas las contradicciones originales resueltas. CLAUDE.md sincronizado.

## Estado global

| Contradicción | Severidad original | Estado | Fecha resolución |
|---|---|---|---|
| Licensing marcado como pendiente | Alta | ✓ Resuelto | 2026-06-04 |
| Docker no documentado | Alta | ✓ Resuelto | 2026-06-04 |
| "9 migraciones" incorrecto | Media | ✓ Resuelto | 2026-06-04 |
| §18 "facturación fuera de alcance" | Baja | ✓ Clarificado | 2026-06-04 |
| "31 tests" y "10 migraciones" desfasados | Media | ✓ Resuelto | 2026-06-05 |
| SalesQuote, informes, paridad Compras/Ventas no en §49.3 | Alta | ✓ Resuelto | 2026-06-05 |
| Vault Obsidian no sincronizado con código | Alta | ✓ Resuelto — protocolo activo | 2026-06-05 |

---

## Protocolo activo desde 2026-06-05

**Al finalizar cada sesión de desarrollo**, actualizar:
1. `CLAUDE.md` §6 (estado real), §46 (roadmap), §47.1 (conflictos), §49.3 (módulos)
2. `00 - Inicio.md` (tests, migraciones, módulos)
3. `08 - Pendientes/Pendientes priorizados.md` (marcar resueltos, añadir nuevos)
4. `08 - Pendientes/Huecos funcionales.md` (marcar implementados)
5. `10 - Auditoría/Inventario técnico.md` (nuevos artefactos)
6. `07 - UI Blazor/Rutas Blazor.md` (nuevas rutas)
7. `09 - Ideas y decisiones.md` (ideas surgidas en la sesión)

---

## Contradicciones potenciales a vigilar

| Tema | Riesgo | Cómo vigilar |
|---|---|---|
| Tests nuevos no reflejados | Bajo — se actualiza por sesión | Comprobar `dotnet test` al inicio |
| Migraciones nuevas no en §6 | Medio — después de cada migración | Actualizar §6 inmediatamente |
| Nuevas rutas Blazor no en vault | Medio — olvidar actualizar vault | Protocolo fin de sesión |
| §47.2 decisiones pendientes | Alto — afectan arquitectura | Revisar antes de implementar multi-tenant o cuentas individuales |
