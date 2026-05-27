# Flujo Git — Debales

## Ramas

No trabajar directamente sobre `main` salvo el commit inicial de cimientos.

### Formato de ramas

```
feature/module-crm-customers
feature/ai-context-builder
fix/license-validation
docs/architecture-overview
refactor/core-permission-service
test/crm-customer-creation
```

### Ramas principales

| Rama      | Propósito                                      |
|-----------|------------------------------------------------|
| `main`    | Código estable, revisado y aprobado            |
| `develop` | Integración de features en curso (opcional)    |

## Commits

Formato: `tipo(scope): descripción`

### Tipos

| Tipo       | Uso                                            |
|------------|------------------------------------------------|
| `feat`     | Nueva funcionalidad                            |
| `fix`      | Corrección de bug                              |
| `docs`     | Documentación                                  |
| `refactor` | Refactoring sin cambio de comportamiento        |
| `test`     | Tests nuevos o modificados                     |
| `chore`    | Mantenimiento, dependencias, configuración     |
| `migration`| Migraciones de base de datos                   |

### Ejemplos

```
feat(crm): add customer entity and repository
feat(ai): add IAIProvider abstraction
fix(core): validate module dependency on load
docs(architecture): add modular system overview
test(crm): add customer creation use case tests
migration(core): add SystemUsers and SystemRoles tables
```

## Pull Requests

Toda feature debe entrar via PR a `main`:

1. Rama creada desde `main`.
2. Código revisado (humano o `/code-review`).
3. Tests pasando.
4. Documentación actualizada si aplica.
5. PR aprobado por al menos un revisor.

## Migraciones

Toda migración debe incluir en su descripción:

- Motivo del cambio.
- Impacto esperado.
- Si es reversible.
- Datos afectados.
- Prueba mínima realizada.
