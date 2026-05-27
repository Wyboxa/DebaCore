# Testing — Debales

## Regla principal

Todo cambio relevante debe tener prueba. Si no se han ejecutado tests, decirlo explícitamente.

## Tipos de tests

| Tipo               | Propósito                                               | Proyecto                          |
|--------------------|---------------------------------------------------------|-----------------------------------|
| Unit tests         | Lógica de dominio y casos de uso aislados               | `Debales.Domain.Tests`            |
| Integration tests  | Repositorios, EF Core, BD real                          | `Debales.Integration.Tests`       |
| API tests          | Endpoints end-to-end                                    | `Debales.Integration.Tests`       |
| AI prompt tests    | Validar respuestas de providers IA en escenarios clave  | `Debales.AI.Tests` (fase futura)  |
| Migration tests    | Verificar que las migraciones se aplican correctamente  | `Debales.Integration.Tests`       |

## Casos críticos a probar siempre

- Permisos (usuario sin permiso no accede).
- Multi-tenant (usuario de tenant A no ve datos de tenant B).
- Activación/desactivación de módulos.
- Migraciones (up y down si aplica).
- Acceso a datos por IA (solo contexto autorizado).
- Expiración y validación de licencia.
- Auditoría (acciones críticas quedan registradas).
- Errores funcionales (respuesta correcta ante input inválido).

## Formato de validación en respuestas

Cuando se entregue código, reportar siempre:

```md
Validación:
- Build: correcto / no ejecutado
- Tests unitarios: correcto (X/X) / no ejecutados
- Tests integración: correcto / no ejecutados
- Revisión manual: realizada sobre [archivos]
```

## Comandos

```bash
dotnet build
dotnet test
dotnet test --filter Category=Unit
dotnet test --filter Category=Integration
```
