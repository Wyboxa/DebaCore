---
type: index
module: cross
layer: cross
status: implemented
related:
  - 00 - Inicio
  - Core
  - CRM
  - Suppliers
  - Catalogo
  - Ventas
  - Compras
  - Facturacion
  - Inventario
  - Contabilidad
  - IA
  - Licenciamiento
  - Despliegue
---

# Índice de módulos

| Módulo | Estado | Migración | Nota |
|--------|--------|-----------|------|
| [[Core]] | Implementado | InitialCreate | Usuarios, Roles, Permisos, Auditoría |
| [[CRM]] | Implementado | AddCrmModule | Clientes, Contactos, Actividades, Notas, Oportunidades |
| [[Suppliers]] | Implementado | AddSuppliersModule | Proveedores con soft-delete |
| [[Catalogo]] | Implementado | AddCatalogModule | Artículos, Familias, UoM, TaxType |
| [[Ventas]] | Implementado | AddERP2Module + AddERP3Module | Pedidos, Albaranes, Facturas, Rectificativas, Cobros |
| [[Compras]] | Implementado | AddERP2Module + AddERP3Module | Pedidos, Albaranes, Facturas, Rectificativas, Pagos |
| [[Facturacion]] | Implementado | AddERP3Module | Entidades de facturación compartidas Ventas/Compras |
| [[Inventario]] | Implementado | AddERP4Module | Almacenes, Ubicaciones, Movimientos, Saldos |
| [[Contabilidad]] | Implementado | AddAccountingModule | PGC España, Ejercicios, Diarios, Asientos |
| [[IA]] | Implementado | — (sin migración propia) | Chat ERP, Anomalías, Resúmenes, Briefing |
| [[Licenciamiento]] | **Implementado** | AddLicensingModule | Planes, Licencias, Módulos — CLAUDE.md lo marca como pendiente |
| [[Despliegue]] | **Implementado** | — | docker-compose.yml + Dockerfiles — CLAUDE.md lo marca como pendiente |
