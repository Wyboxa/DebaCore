# Debales — CRM/ERP Modular con IA Supervisada

Plataforma empresarial modular construida en .NET 8, donde la IA analiza, propone y documenta cambios bajo supervisión humana.

## Tecnologías

- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core, SQL Server
- **Frontend**: Blazor Server
- **IA**: Módulo supervisado con proveedor configurable (Claude, OpenAI, Mock)
- **Tests**: xUnit — 52 tests automatizados
- **Despliegue**: Docker Compose (opcional)

## Requisitos para ejecutar en local

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server LocalDB (incluido en Visual Studio) o SQL Server
- Git

## Estructura del proyecto

```
src/
├── Debales.Api/           API REST (ASP.NET Core)
├── Debales.Web/           UI Blazor Server
├── Debales.Application/   Casos de uso, handlers, DTOs
├── Debales.Domain/        Entidades, value objects, reglas de dominio
├── Debales.Infrastructure/ EF Core, repositorios, persistencia
└── Debales.AI/            Orquestación IA

tests/
├── Debales.Domain.Tests/
├── Debales.Application.Tests/
└── Debales.Integration.Tests/

docs/
├── architecture/          Documentos de arquitectura
├── decisions/             ADRs (Architecture Decision Records)
└── modules/               Documentación por módulo
```

## Módulos implementados

| Módulo | Estado |
|--------|--------|
| Core (Users, Roles, Permissions) | Completo |
| CRM (Clientes, Contactos, Actividades) | Completo |
| Suppliers (Proveedores) | Completo |
| Catalog (Artículos, Familias, IVA) | Completo |
| Sales (Presupuestos, Pedidos, Albaranes, Facturas, Cobros) | Completo |
| Purchasing (Pedidos, Albaranes, Facturas, Pagos) | Completo |
| Inventory (Almacenes, Movimientos, Saldos) | Completo |
| Accounting (Plan contable, Asientos, Informes) | Completo |
| AI supervisada (Chat ERP, anomalías, resúmenes) | Completo |
| Licensing (Licencias, planes, módulos) | Completo |
| Docker Compose | Completo |

## Configuración local

### 1. Clonar el repositorio

```bash
git clone https://github.com/Wyboxa/DebaCore.git
cd DebaCore
```

### 2. Configurar appsettings de desarrollo

Copiar las plantillas de configuración para cada proyecto:

```bash
copy src\Debales.Api\appsettings.Development.example.json src\Debales.Api\appsettings.Development.json
copy src\Debales.Web\appsettings.Development.example.json src\Debales.Web\appsettings.Development.json
```

Editar ambos archivos con tus valores locales:

| Campo | Descripción |
|-------|-------------|
| `Jwt:Secret` | Cadena aleatoria de mínimo 32 caracteres |
| `AI:Provider` | `Mock` (sin API key) o `Claude` / `OpenAI` |
| `AI:ApiKey` | API key del proveedor elegido (opcional si Mock) |

**Alternativa recomendada para secretos**: usar `dotnet user-secrets`:

```bash
cd src/Debales.Api
dotnet user-secrets set "Jwt:Secret" "tu-secreto-min-32-chars"
dotnet user-secrets set "AI:ApiKey" "tu-api-key"
```

### 3. Aplicar migraciones de base de datos

```powershell
dotnet ef database update `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj
```

Esto crea la base de datos en LocalDB (`Server=(localdb)\MSSQLLocalDB;Database=Debales`).

### 4. Ejecutar el proyecto

**API** (puerto 5000/5001 por defecto):

```bash
dotnet run --project src/Debales.Api
```

**Web** (puerto 5291 por defecto):

```bash
dotnet run --project src/Debales.Web
```

Abrir en el navegador: `http://localhost:5291`

### 5. Ejecutar tests

```bash
dotnet test
```

## Despliegue con Docker

Copiar `.env.example` como `.env` y rellenar los valores:

```bash
copy .env.example .env
```

Editar `.env`:

| Variable | Descripción |
|----------|-------------|
| `SA_PASSWORD` | Contraseña de SQL Server (mín. 8 chars, mayúsculas, números, símbolo) |
| `JWT_SECRET` | Secreto JWT de producción (mín. 32 chars) |
| `AI_PROVIDER` | `Mock`, `Claude`, o `OpenAI` |
| `AI_APIKEY` | API key del proveedor IA (vacío si Mock) |
| `WEB_PORT` | Puerto para la UI (defecto: 8080) |
| `API_PORT` | Puerto para la API (defecto: 5001) |

Iniciar los contenedores:

```bash
docker-compose up -d
```

## Migraciones EF Core

**Crear nueva migración:**

```powershell
dotnet ef migrations add NombreMigracion `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj
```

**Aplicar migraciones pendientes:**

```powershell
dotnet ef database update `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj
```

> Usar siempre `Debales.Api` como `--startup-project`. `Debales.Web` no tiene el paquete de diseño EF.

## Archivos que NO están en el repositorio

Por seguridad, los siguientes archivos se ignoran y deben crearse localmente:

| Archivo | Plantilla disponible |
|---------|---------------------|
| `src/Debales.Api/appsettings.Development.json` | `appsettings.Development.example.json` |
| `src/Debales.Web/appsettings.Development.json` | `appsettings.Development.example.json` |
| `.env` | `.env.example` |
| `src/**/Properties/launchSettings.json` | Generado por el IDE |

## Convenciones del proyecto

- Respuestas y documentación: **español**
- Código, commits, namespaces: **inglés**
- LINQ: sintaxis `from … in … select` para queries complejas
- Entidades de dominio: `sealed class`
- DTOs y value objects: `record`
- Sin abstracciones no justificadas — MVP primero
