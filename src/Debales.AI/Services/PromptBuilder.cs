using System.Text;
using Debales.Application.AI;
using Debales.Application.AI.ERP;
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

    // ── ERP-6 ──────────────────────────────────────────────────────────────

    internal static string BuildERPSystemPrompt(ERPAIContext ctx)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Eres el asistente ERP de Debales. Ayudas al equipo financiero y de gestión con información sobre ventas, compras, cobros, pagos y contabilidad.");
        sb.AppendLine("Responde en español. Sé preciso con los importes. Usa bullet points. No inventes datos que no estén en el contexto.");
        sb.AppendLine();

        sb.AppendLine($"## VENTAS — Mes actual: {ctx.TotalSalesThisMonth:N2} € | Mes anterior: {ctx.TotalSalesLastMonth:N2} €");
        sb.AppendLine($"- Facturas contabilizadas (últimas {ctx.RecentSalesInvoices.Count}): {ctx.RecentSalesInvoices.Sum(i => i.Total):N2} € total");
        if (ctx.DraftSalesInvoicesCount > 0)
            sb.AppendLine($"- ⚠️ {ctx.DraftSalesInvoicesCount} factura(s) de venta en borrador sin contabilizar");

        if (ctx.PendingReceivables.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## COBROS PENDIENTES — {ctx.TotalPendingReceivables:N2} € total ({ctx.OverdueReceivablesCount} vencidos)");
            foreach (var r in ctx.PendingReceivables.OrderBy(r => r.DueDate).Take(8))
                sb.AppendLine($"- {r.Customer} | {r.Amount:N2} € | Vto: {r.DueDate:dd/MM/yyyy}{(r.IsOverdue ? " ⚠️ VENCIDO" : "")}");
        }

        sb.AppendLine();
        sb.AppendLine($"## COMPRAS — Mes actual: {ctx.TotalPurchasesThisMonth:N2} € | Mes anterior: {ctx.TotalPurchasesLastMonth:N2} €");
        if (ctx.DraftPurchaseInvoicesCount > 0)
            sb.AppendLine($"- ⚠️ {ctx.DraftPurchaseInvoicesCount} factura(s) de compra en borrador sin contabilizar");

        if (ctx.PendingPayables.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## PAGOS PENDIENTES — {ctx.TotalPendingPayables:N2} € total ({ctx.OverduePayablesCount} vencidos)");
            foreach (var p in ctx.PendingPayables.OrderBy(p => p.DueDate).Take(8))
                sb.AppendLine($"- {p.Supplier} | {p.Amount:N2} € | Vto: {p.DueDate:dd/MM/yyyy}{(p.IsOverdue ? " ⚠️ VENCIDO" : "")}");
        }

        return sb.ToString();
    }

    internal static string BuildCustomerERPSystemPrompt(CustomerERPContext ctx)
    {
        var sb = new StringBuilder();
        sb.Append(BuildSystemPrompt(new CustomerAIContext(
            ctx.Customer, ctx.Contacts, ctx.RecentActivities, ctx.RecentNotes, ctx.Opportunities)));

        if (ctx.RecentInvoices.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## FACTURACIÓN (últimas {ctx.RecentInvoices.Count} facturas) — Total: {ctx.TotalBilled:N2} €");
            foreach (var i in ctx.RecentInvoices.Take(6))
                sb.AppendLine($"- {i.Number} | {i.Date:dd/MM/yyyy} | {i.Total:N2} € | {i.Status}");
        }

        if (ctx.PendingReceivables.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## COBROS PENDIENTES — {ctx.TotalPending:N2} €");
            foreach (var r in ctx.PendingReceivables.OrderBy(r => r.DueDate))
                sb.AppendLine($"- {r.Number} | Vto: {r.DueDate:dd/MM/yyyy} | {r.Amount:N2} €{(r.IsOverdue ? " ⚠️ VENCIDO" : "")}");
        }

        return sb.ToString();
    }

    internal static string BuildCustomerERPSummaryPrompt() =>
        "Genera un resumen financiero completo del cliente en 4-6 puntos clave. Incluye: volumen de negocio, estado de cobros, relación comercial, riesgo de impago si aplica, y próximas acciones recomendadas. Sé directo, usa cifras concretas.";

    internal static string BuildSupplierERPSystemPrompt(SupplierAIContext ctx)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Eres el asistente ERP de Debales. Analizas la relación financiera con proveedores.");
        sb.AppendLine("Responde en español. Sé preciso con los importes.");
        sb.AppendLine();

        var s = ctx.Supplier;
        sb.AppendLine($"## PROVEEDOR: {s.Name}");
        sb.AppendLine($"- CIF: {s.TaxId ?? "No especificado"} | Contacto: {s.ContactName ?? "—"} | Tel: {s.Phone ?? "—"}");

        if (ctx.RecentInvoices.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## FACTURAS DE COMPRA ({ctx.RecentInvoices.Count}) — Total: {ctx.TotalPurchased:N2} €");
            foreach (var i in ctx.RecentInvoices.Take(6))
                sb.AppendLine($"- {i.Number} | {i.Date:dd/MM/yyyy} | {i.Total:N2} € | {i.Status}");
        }

        if (ctx.PendingPayables.Any())
        {
            sb.AppendLine();
            sb.AppendLine($"## PAGOS PENDIENTES — {ctx.TotalPayable:N2} €");
            foreach (var p in ctx.PendingPayables.OrderBy(p => p.DueDate))
                sb.AppendLine($"- {p.Number} | Vto: {p.DueDate:dd/MM/yyyy} | {p.Amount:N2} €{(p.IsOverdue ? " ⚠️ VENCIDO" : "")}");
        }

        return sb.ToString();
    }

    internal static string BuildSupplierERPSummaryPrompt() =>
        "Genera un resumen de la relación con este proveedor en 3-5 puntos: volumen de compras, pagos pendientes, riesgo de retraso y recomendaciones de gestión.";
}
