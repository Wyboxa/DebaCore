---
type: module
module: despliegue
layer: infrastructure
status: implemented
source:
  - docker-compose.yml
  - Dockerfile.api
  - Dockerfile.web
related:
  - 01 - Arquitectura
---

# Módulo Despliegue

## AVISO: Contradicción con CLAUDE.md

CLAUDE.md §30 (Fase 7) declara Docker Compose como pendiente. El código confirma que está implementado con `docker-compose.yml`, `Dockerfile.api` y `Dockerfile.web` en la raíz del proyecto. Ver [[Contradicciones detectadas]].

## Estado

**Implementado** — docker-compose.yml + Dockerfile.api + Dockerfile.web presentes en raíz.

## Servicios Docker

| Servicio | Imagen | Puerto | Descripción |
|----------|--------|--------|-------------|
| `sqlserver` | `mcr.microsoft.com/mssql/server:2022-latest` | interno | SQL Server 2022 con healthcheck |
| `api` | Build `Dockerfile.api` | `${API_PORT:-5001}:8080` | API REST ASP.NET Core |
| `web` | Build `Dockerfile.web` | `${WEB_PORT:-8080}:8080` | Blazor Server UI |

## Red y volúmenes

- Red: `debales-net` (bridge)
- Volumen: `debales-sql-data` (persistencia de datos SQL Server)

## Variables de entorno requeridas

| Variable | Ejemplo | Descripción |
|----------|---------|-------------|
| `SA_PASSWORD` | `Debales2026!` | Contraseña SA de SQL Server |
| `JWT_SECRET` | clave larga | Clave de firma JWT |
| `AI_PROVIDER` | `Mock` o `Claude` | Proveedor de IA |
| `AI_APIKEY` | clave Anthropic | API key de Claude (opcional si Mock) |
| `AI_MODEL` | `claude-sonnet-4-6` | Modelo de IA |
| `API_PORT` | `5001` | Puerto externo de la API |
| `WEB_PORT` | `8080` | Puerto externo de la UI |

## Comportamiento en startup

1. SQL Server arranca con healthcheck
2. `api` espera a que SQL Server esté healthy
3. Al arrancar, `api` ejecuta `MigrateAsync()` + `DbSeeder.SeedAsync()` + `DemoDataSeeder.SeedAsync()`
4. `web` también espera SQL Server healthy

## Lo que está completo

- docker-compose.yml funcional con 3 servicios
- Healthcheck de SQL Server
- Dependencias entre servicios con `condition: service_healthy`
- Auto-migración en startup
- Configuración por variables de entorno

## Lo que falta

- Archivo `.env.example` para documentar variables
- Compose para entorno de desarrollo con hot reload
- Nginx como reverse proxy (no confirmado)
- CLAUDE.md debe actualizarse para reflejar que está implementado
