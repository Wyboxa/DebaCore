---
type: database
module: cross
layer: database
status: implemented
source:
  - src/Debales.Infrastructure/Persistence/DbSeeder.cs
  - src/Debales.Infrastructure/Persistence/DemoDataSeeder.cs
  - src/Debales.Infrastructure/Persistence/Seeders/CatalogSeeder.cs
  - src/Debales.Infrastructure/Persistence/Configurations/Accounting/AccountingSeeds.cs
related:
  - DbContext
  - Migraciones EF Core
---

# Seeds

## DbSeeder

**Archivo**: `src/Debales.Infrastructure/Persistence/DbSeeder.cs`  
**Ejecutado por**: `Program.cs` en startup (`DbSeeder.SeedAsync(context, hasher)`)  
**Idempotente**: Sí (comprueba si ya existen datos antes de sembrar)

### Datos sembrados

**Roles:**
- `Admin` — Administrador del sistema (isSystem: true)
- `User` — Usuario estándar (isSystem: true)

**Usuario admin:**
- Email: `admin@debales.local`
- Password: `Admin1234!`
- Rol: Admin

---

## DemoDataSeeder

**Archivo**: `src/Debales.Infrastructure/Persistence/DemoDataSeeder.cs`  
**Ejecutado por**: `Program.cs` en startup  
**Sentinel**: `Customers.Any(c => c.Name == "Construcciones Herrera S.L.")`

### Datos sembrados

**Artículos:**

| Código | Nombre | Tipo | PVP | PVP Compra | IVA |
|--------|--------|------|-----|------------|-----|
| TUB-001 | Tubo acero galvanizado DN50 | Producto | 18.50 | 12.00 | 21% |
| VAL-001 | Válvula de corte DN50 | Producto | 45.00 | 28.00 | 21% |
| CEM-001 | Cemento Portland 25kg | Producto | 6.80 | 4.20 | 10% |
| SVC-INS | Instalación hidráulica | Servicio | 55.00 | 0 | 21% |
| SVC-MNT | Mantenimiento preventivo | Servicio | 48.00 | 0 | 21% |

**Clientes:**

| Nombre | NIF | Sector |
|--------|-----|--------|
| Construcciones Herrera S.L. | B12345678 | Construcción |
| Servicios Norte S.A. | A87654321 | Mantenimiento |
| Talleres Pérez e Hijos | B99887766 | Industria |

**Almacén:**
- `ALM-01` "Almacén Principal Sevilla"

**Stock inicial:**
- TUB-001: 500 UN en ALM-01
- VAL-001: 150 UN en ALM-01
- CEM-001: 2000 KG en ALM-01

**Escenarios de venta:**

| Escenario | Pedido | Estado | Descripción |
|-----------|--------|--------|-------------|
| A | PV-2026-0001 | Confirmed | Herrera — pendiente albarán |
| B | PV-2026-0002 | Delivered | Norte — albarán ALV-2026-0001 emitido |
| C | PV-2026-0003 | Delivered | Pérez — flujo completo hasta FV-2026-0001 Posted |

---

## CatalogSeeder

**Archivo**: `src/Debales.Infrastructure/Persistence/Seeders/CatalogSeeder.cs`  
(Referenciado en código pero la función `EnsureCatalogAsync` en DemoDataSeeder hace lo mismo)

Siembra:
- UoM: UN, KG, H
- TaxTypes: IVA21 (21%), IVA10 (10%)
- ItemFamilies: PROD, SERV

---

## AccountingSeeds

**Archivo**: `src/Debales.Infrastructure/Persistence/Configurations/Accounting/AccountingSeeds.cs`

Define GUIDs estables para datos de referencia contables sembrados vía EF data seeding (HasData):

- 4 Diarios: VTA, CPR, BCO, CAJ
- 14 Cuentas PGC: 300, 400, 430, 472, 475, 477, 570, 572, 600, 621, 628, 640, 700, 705
- 2 Plantillas de asiento: SalesInvoicePosted, PurchaseInvoicePosted
- 6 Líneas de plantilla predefinidas
