# Módulo CRM — Debales

## Estado

Planificado — Fase 3

## Dependencias

- Core 1.0.0

## Propósito

Primer módulo funcional real. Gestión de relaciones con clientes.

## Funcionalidades previstas

### Clientes

- Ficha de cliente (nombre, sector, dirección, CIF, estado).
- Búsqueda y filtrado.
- Historial de actividad.

### Contactos

- Contactos asociados a un cliente.
- Datos de contacto (nombre, cargo, email, teléfono).

### Actividades

- Registro de llamadas, reuniones, emails y tareas.
- Asociación a cliente y contacto.
- Estado (pendiente, realizada).

### Notas

- Notas internas sobre clientes.
- Autor y fecha.

### Oportunidades (simplificado)

- Oportunidad de venta asociada a cliente.
- Estado básico (nueva, en curso, ganada, perdida).
- Valor estimado.

### Búsqueda

- Búsqueda global por cliente, contacto, actividad.

## Tablas principales (propuesta)

```
CrmCustomers
CrmContacts
CrmActivities
CrmNotes
CrmOpportunities
```

## Permisos del módulo

```
crm.customers.read
crm.customers.write
crm.contacts.read
crm.contacts.write
crm.activities.read
crm.activities.write
crm.notes.read
crm.notes.write
crm.opportunities.read
crm.opportunities.write
```
