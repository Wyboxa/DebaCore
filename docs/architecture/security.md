# Seguridad y privacidad — Debales

## Datos locales

El producto está diseñado para que los datos del cliente puedan quedarse en su infraestructura.

Reglas:

- No enviar datos del cliente a proveedores externos sin consentimiento explícito.
- No mezclar datos de diferentes clientes.
- No usar datos reales para entrenar modelos sin contrato explícito.
- No registrar información sensible en logs sin anonimización.
- No exponer secretos en código ni en repositorio.

## Secretos

Nunca guardar en el repositorio:

- API keys.
- Passwords.
- Connection strings reales.
- Tokens.
- Certificados privados.

Usar:

- Variables de entorno.
- Secret Manager (.NET).
- `appsettings.local.json` ignorado por Git.

## Permisos

El sistema contempla:

- Usuarios con roles.
- Roles con permisos granulares.
- Módulos activos por cliente.
- Acciones auditables.
- Separación por Tenant si aplica.

## Auditoría

Acciones críticas auditables:

- Alta, modificación y eliminación de datos clave.
- Cambios de configuración.
- Activación/desactivación de módulos.
- Acciones IA relevantes y sus aprobaciones humanas.
- Cambios propuestos por IA.
