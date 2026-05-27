# Proceso de release — Debales

## Versionado

SemVer por módulo: `MAJOR.MINOR.PATCH`

| Incremento | Cuándo                                                   |
|------------|----------------------------------------------------------|
| MAJOR      | Cambio que rompe compatibilidad                          |
| MINOR      | Nueva funcionalidad compatible hacia atrás               |
| PATCH      | Corrección de bugs compatible hacia atrás                |

## Versiones iniciales

```
Core       1.0.0
CRM        1.0.0
Documents  1.0.0
AI         1.0.0
```

## Checklist antes de un release

- [ ] Tests unitarios pasando (`dotnet test`).
- [ ] Build limpio (`dotnet build`).
- [ ] Migraciones revisadas y probadas en Staging.
- [ ] Documentación actualizada.
- [ ] Notas de versión escritas.
- [ ] Aprobación humana explícita.
- [ ] Tag de versión creado en Git.

## Notas de versión

Formato mínimo:

```md
# Debales vX.Y.Z — [Módulo] — [Fecha]

## Cambios
- [Cambio 1]
- [Cambio 2]

## Migraciones incluidas
- [Migración]

## Riesgos conocidos
- [Riesgo si aplica]

## Instrucciones de actualización
- [Paso si aplica]
```

## Despliegue

- La IA no despliega en producción directamente.
- Todo despliegue requiere aprobación humana explícita.
- Staging se valida antes que Production.
