using System.Text;
using Debales.Application.AI;
using Debales.Application.CRM.Dashboard;

namespace Debales.AI.Services;

internal static class PromptBuilder
{
    internal static string BuildSystemPrompt(CustomerAIContext ctx)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Eres el asistente IA de Debales CRM. Tu misión es ayudar al equipo comercial con información sobre el cliente y sugerir acciones de venta.");
        sb.AppendLine("Responde siempre en español. Sé conciso, preciso y orientado a negocio. No inventes datos que no estén en el contexto.");
        sb.AppendLine();

        var c = ctx.Customer;
        sb.AppendLine($"## CLIENTE: {c.Name}");
        sb.AppendLine($"- Sector: {c.Sector ?? "No especificado"}");
        sb.AppendLine($"- CIF/NIF: {c.TaxId ?? "No especificado"}");
        sb.AppendLine($"- Teléfono: {c.Phone ?? "No especificado"}");
        sb.AppendLine($"- Web: {c.Website ?? "No especificada"}");
        sb.AppendLine($"- Estado: {(c.IsActive ? "Activo" : "Inactivo")}");
        sb.AppendLine($"- Alta: {c.CreatedAt:dd/MM/yyyy}");

        if (ctx.Contacts.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## CONTACTOS ({ctx.Contacts.Count})");
            foreach (var contact in ctx.Contacts.Where(x => x.IsActive))
                sb.AppendLine($"- {contact.FullName}{(contact.JobTitle is not null ? $" ({contact.JobTitle})" : "")} — {contact.Email ?? "sin email"} — {contact.Phone ?? "sin teléfono"}");
        }

        var openOpps = ctx.Opportunities.Where(o => o is { Status: not "Won" and not "Lost" }).ToList();
        if (ctx.Opportunities.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## OPORTUNIDADES ({ctx.Opportunities.Count} total, {openOpps.Count} abiertas)");
            foreach (var o in ctx.Opportunities.OrderByDescending(x => x.CreatedAt).Take(5))
            {
                var valor = o.EstimatedValue.HasValue ? $"{o.EstimatedValue:N0} €" : "sin valor";
                var cierre = o.ExpectedCloseDate.HasValue ? o.ExpectedCloseDate.Value.ToString("dd/MM/yyyy") : "sin fecha";
                sb.AppendLine($"- [{o.Status}] {o.Title} — {valor} — Cierre: {cierre}");
            }
        }

        var recentActivities = ctx.RecentActivities.OrderByDescending(a => a.ScheduledAt).Take(5).ToList();
        if (recentActivities.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## ACTIVIDADES RECIENTES (últimas {recentActivities.Count})");
            foreach (var a in recentActivities)
                sb.AppendLine($"- {a.ScheduledAt:dd/MM/yyyy} [{a.Type}] {a.Subject}{(a.IsCompleted ? " ✓" : "")}");
        }

        var recentNotes = ctx.RecentNotes.OrderByDescending(n => n.CreatedAt).Take(3).ToList();
        if (recentNotes.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## NOTAS RECIENTES (últimas {recentNotes.Count})");
            foreach (var n in recentNotes)
                sb.AppendLine($"- {n.CreatedAt:dd/MM/yyyy}: {n.Content}");
        }

        return sb.ToString();
    }

    internal static string BuildSummaryPrompt() =>
        "Genera un resumen ejecutivo del cliente en 3-5 puntos clave. Incluye: estado de la relación comercial, oportunidades destacadas, próximas acciones recomendadas y cualquier dato relevante del historial. Sé directo y orientado a negocio.";

    internal static string BuildDashboardSystemPrompt() =>
        "Eres el asistente comercial IA de Debales CRM. Generas briefings diarios concisos para el equipo de ventas. Responde siempre en español. Usa bullet points (•). Máximo 5 puntos. Sin introducciones ni despedidas. Solo la información accionable.";

    internal static string BuildDashboardBriefingMessage(DashboardStatsDto stats)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Datos del CRM a fecha de hoy ({DateTime.Now:dd/MM/yyyy}):");
        sb.AppendLine($"- Clientes activos: {stats.ActiveCustomers} de {stats.TotalCustomers}");
        sb.AppendLine($"- Oportunidades abiertas: {stats.OpenOpportunities} — Valor pipeline: {stats.PipelineValue:N0} €");
        sb.AppendLine($"- Actividades pendientes: {stats.PendingActivities} ({stats.OverdueActivities} vencidas sin completar)");

        if (stats.UpcomingActivities.Any())
        {
            sb.AppendLine("- Próximas actividades (14 días):");
            foreach (var a in stats.UpcomingActivities.Take(5))
                sb.AppendLine($"  · {a.ScheduledAt:dd/MM} [{a.Type}] {a.Subject} — {a.CustomerName}{(a.IsOverdue ? " ⚠️ VENCIDA" : "")}");
        }

        if (stats.Pipeline.Any())
        {
            sb.AppendLine("- Pipeline por estado:");
            foreach (var p in stats.Pipeline)
                sb.AppendLine($"  · {p.Stage}: {p.Count} oportunidades ({p.Value:N0} €)");
        }

        sb.AppendLine();
        sb.AppendLine("Genera el briefing del día con las prioridades más importantes para el equipo comercial.");
        return sb.ToString();
    }
}
