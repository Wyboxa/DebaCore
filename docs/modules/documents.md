# Módulo Documents — Debales

## Estado

Pendiente de implementar — Sin fase ERP asignada (backlog post ERP-2)

## Dependencias

- Core 1.0.0
- CRM 1.0.0 (para documentos asociados a clientes)

## Propósito

Gestión de documentos vinculados a entidades del sistema (clientes, contratos, propuestas).

## Funcionalidades previstas

### Documentos

- Subida y almacenamiento de archivos.
- Asociación a cliente u otras entidades.
- Clasificación por tipo (contrato, propuesta, factura, otro).

### Versionado básico

- Historial de versiones de cada documento.
- Descarga de versión específica.

### Comentarios

- Comentarios internos sobre documentos.

### Búsqueda

- Búsqueda por nombre, tipo, cliente, fecha.

## Tablas principales (propuesta)

```
Documents
DocumentVersions
DocumentComments
DocumentCategories
```

## Permisos del módulo

```
documents.read
documents.write
documents.delete
documents.versions.read
documents.comments.read
documents.comments.write
```
