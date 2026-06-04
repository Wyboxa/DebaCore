---
type: diagram
module: cross
layer: cross
status: implemented
related:
  - Índice de módulos
  - Arquitectura general
---

# Diagrama: Relación de módulos

```mermaid
graph TD
    Core[Core\nUsuarios + Roles + Auditoría]
    CRM[CRM\nClientes + Contactos + Oportunidades]
    Suppliers[Suppliers\nProveedores]
    Catalog[Catálogo\nArtículos + Familias + UoM + IVA]
    Sales[Ventas\nPedidos + Albaranes]
    Purchasing[Compras\nPedidos + Albaranes]
    Billing[Facturación\nFacturas + Rectificativas + Cobros]
    Inventory[Inventario\nAlmacenes + Stock]
    Accounting[Contabilidad\nPGC + Ejercicios + Asientos]
    AI[IA ERP-6\nChat + Anomalías + Resúmenes]
    Licensing[Licenciamiento\nPlanes + Licencias]

    Core --> CRM
    Core --> Suppliers
    Core --> Catalog
    Core --> Licensing

    Catalog --> Sales
    Catalog --> Purchasing
    CRM --> Sales
    Suppliers --> Purchasing

    Sales --> Billing
    Purchasing --> Billing

    Sales --> Inventory
    Purchasing --> Inventory

    Billing --> Accounting

    Sales --> AI
    Purchasing --> AI
    Accounting --> AI
    CRM --> AI
    Suppliers --> AI
```

## Leyenda

- Las flechas indican dependencia de datos (el módulo origen proporciona datos al destino)
- **Core** es fundamento de todos los módulos
- **Catalog** es necesario para crear líneas de pedidos y facturas
- **IA** consume datos de todos los módulos (solo lectura)
