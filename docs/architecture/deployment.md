# Despliegue — Debales

## Escenarios previstos

| Escenario                    | Descripción                                          |
|------------------------------|------------------------------------------------------|
| Local/on-premise             | Instalado en infraestructura del cliente             |
| Servidor privado del cliente | Servidor dedicado gestionado por el cliente          |
| Nube privada                 | Nube gestionada por Debales para el cliente          |
| SaaS                         | Plataforma compartida gestionada por Debales         |

El diseño permite evolución entre escenarios sin bloquear al cliente.

## Fase 7 — Despliegue local (planificado)

- Docker Compose como opción preferente para instalación on-premise.
- Configuración por cliente mediante variables de entorno o archivo local.
- Backups automatizables.
- Proceso de actualización documentado.

## Convenciones actuales

- Entornos: Development, Staging, Production.
- La IA nunca despliega en Production directamente.
- Toda migración de BD se ejecuta con revisión previa.
- Staging como entorno de validación antes de Production.
