---
type: module
module: documents
layer: cross
status: implemented
source:
  - src/Debales.Domain/Documents/
  - src/Debales.Application/Documents/
  - src/Debales.Infrastructure/Persistence/Configurations/Documents/
  - src/Debales.Infrastructure/Persistence/Repositories/Documents/
  - src/Debales.Api/Controllers/DocumentsController.cs
  - src/Debales.Api/Controllers/DocumentTypesController.cs
  - src/Debales.Web/Components/Pages/Documentos/
related:
  - Customer
  - Supplier
---

# Módulo Documents

## Qué problema resuelve

Gestión de documentos vinculados a clientes o proveedores: contratos, facturas externas, albaranes PDF, imágenes, etc. Permite clasificar por tipo, asociar al tercero correspondiente y archivar con metadatos de fichero.

## Estado

Implementado — migración manual `20260613120000_AddDocumentsModule`.

## Entidades del dominio

| Entidad | Descripción |
|---------|-------------|
| `Document` | Documento con título, tipo, fecha, archivo y vínculo a cliente/proveedor |
| `DocumentType` | Clasificación de documentos (Contrato, Factura, Albarán, etc.) |

## Handlers de Application (8)

| Handler | Acción |
|---------|--------|
| `CreateDocumentHandler` | Crea documento verificando que el tipo existe |
| `UpdateDocumentHandler` | Actualiza campos del documento |
| `DeactivateDocumentHandler` | Soft-delete del documento |
| `GetDocumentsHandler` | Lista paginada con filtros por search/tipo/cliente/proveedor |
| `GetDocumentByIdHandler` | Obtiene documento con nombre de tipo, cliente y proveedor |
| `CreateDocumentTypeHandler` | Crea tipo de documento |
| `UpdateDocumentTypeHandler` | Actualiza tipo de documento |
| `GetDocumentTypesHandler` | Lista todos los tipos activos |

## Controllers de API

| Controller | Ruta base | Endpoints |
|------------|-----------|-----------|
| `DocumentsController` | `/api/documents` | GET (lista), GET `/{id}`, POST, PUT `/{id}`, DELETE `/{id}` |
| `DocumentTypesController` | `/api/document-types` | GET, POST, PUT `/{id}` |

## Páginas Blazor

| Página | Ruta | Descripción |
|--------|------|-------------|
| `Documentos.razor` | `/documentos` | Lista paginada con búsqueda y filtros |
| `DocumentoDetalle.razor` | `/documentos/{id}` | Ficha completa con edición |
| `TiposDocumento.razor` | `/configuracion/tipos-documento` | CRUD tipos de documento |

## Integración con fichas

- Tab "Documentos" en `CustomerDetail.razor` — lazy-load filtrado por `CustomerId`
- Tab "Documentos" en `SupplierDetail.razor` — lazy-load filtrado por `SupplierId`

## Campos de `Document`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Title` | `nvarchar(255)` | Título del documento (obligatorio) |
| `Description` | `nvarchar(500)?` | Descripción opcional |
| `DocumentTypeId` | `uniqueidentifier` | FK a DocumentType |
| `CustomerId` | `uniqueidentifier?` | FK opcional a Customers |
| `SupplierId` | `uniqueidentifier?` | FK opcional a Suppliers |
| `FileName` | `nvarchar(500)?` | Nombre original del fichero |
| `FileSizeBytes` | `bigint?` | Tamaño en bytes |
| `MimeType` | `nvarchar(100)?` | Tipo MIME del fichero |
| `Notes` | `nvarchar(1000)?` | Notas adicionales |
| `DocumentDate` | `datetime2` | Fecha del documento (sin hora) |
| `IsActive` | `bit` | Soft-delete |

## Lo que está completo

- CRUD documentos + CRUD tipos de documento
- Vinculación a clientes y proveedores
- Tabs lazy en fichas de Customer y Supplier
- NavMenu: sección "Documentos" + sub-enlace "Tipos de documento" en Configuración

## Lo que falta

- Subida real de ficheros (actualmente solo se almacenan metadatos del fichero)
- Previsualización de PDF en la ficha
- Versionado de documentos (`DocumentVersion`)
- Plantillas de documentos (`DocumentTemplate`)
