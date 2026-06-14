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
  - AIGovernance
  - Documentos
  - Licenciamiento
  - Despliegue
---

# Índice de módulos

| Módulo | Estado | Migración | Nota |
|--------|--------|-----------|------|
| [[Core]] | Implementado | InitialCreate | Usuarios, Roles, Permisos, Auditoría, NumberSeries |
| [[CRM]] | Implementado | AddCrmModule | Clientes, Contactos, Actividades, Notas, Oportunidades |
| [[Suppliers]] | Implementado | AddSuppliersModule | Proveedores, SupplierContact |
| [[Catalogo]] | Implementado | AddCatalogModule + AddPriceListModule | Artículos, Familias, UoM, TaxType, PriceList, ItemPrice, códigos por tercero |
| [[Ventas]] | Implementado | AddERP2Module + AddERP3Module + AddSalesQuoteModule | Presupuestos, Pedidos, Albaranes, Facturas, Rectificativas, Cobros |
| [[Compras]] | Implementado | AddERP2Module + AddERP3Module | Pedidos, Albaranes, Facturas, Rectificativas, Pagos |
| [[Facturacion]] | Implementado | AddERP3Module + AddPaymentMethodModule | Entidades compartidas Ventas/Compras, PaymentTerm, PaymentMethod |
| [[Inventario]] | Implementado | AddERP4Module + AddInventoryCountModule | Almacenes, Ubicaciones, Movimientos, Saldos, Recuentos físicos |
| [[Contabilidad]] | Implementado | AddAccountingModule + AddBankAccountModule + AddCashAccountModule + AddRemittanceModule | PGC España, Asientos, Cuentas bancarias/caja, Remesas, Vencimientos aging, Tesorería |
| [[IA]] | Implementado | — (sin migración propia) | Chat ERP, Anomalías, Resúmenes, Briefing |
| [[AIGovernance]] | **Implementado** | AddAIGovernanceModule (manual) | AIRule, AIKnowledgeBase, AIActionProposal, AIActionApproval, AIExecutionLog |
| [[Documentos]] | **Implementado** | AddDocumentsModule (manual) | Document, DocumentType + tabs en Customer/Supplier |
| [[Licenciamiento]] | **Implementado** | AddLicensingModule | Planes, Licencias, Módulos |
| [[Despliegue]] | **Implementado** | — | docker-compose.yml + Dockerfiles |
