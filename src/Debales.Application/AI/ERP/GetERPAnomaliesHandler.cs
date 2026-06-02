namespace Debales.Application.AI.ERP;

public sealed record GetERPAnomaliesQuery;

public sealed record ERPAnomalyDto(
    string Type, string Severity, string Description, string? Action);

public sealed class GetERPAnomaliesHandler
{
    private readonly IAIService _ai;
    private readonly ChatWithERPHandler _contextBuilder;

    public GetERPAnomaliesHandler(IAIService ai, ChatWithERPHandler contextBuilder)
    {
        _ai = ai;
        _contextBuilder = contextBuilder;
    }

    public async Task<IReadOnlyList<ERPAnomalyDto>> Handle(GetERPAnomaliesQuery query, CancellationToken ct = default)
    {
        var context = await _contextBuilder.BuildContextAsync(ct);

        // Anomalías detectadas con reglas deterministas (sin IA — rápido y fiable)
        var anomalies = new List<ERPAnomalyDto>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (context.OverdueReceivablesCount > 0)
            anomalies.Add(new ERPAnomalyDto(
                "Cobros vencidos", "Alta",
                $"{context.OverdueReceivablesCount} vencimiento(s) de cobro con fecha superada. Total: {context.TotalPendingReceivables:N2} €",
                "Revisar y gestionar cobro de los vencimientos pendientes."));

        if (context.OverduePayablesCount > 0)
            anomalies.Add(new ERPAnomalyDto(
                "Pagos vencidos", "Alta",
                $"{context.OverduePayablesCount} vencimiento(s) de pago con fecha superada.",
                "Procesar pagos pendientes para evitar cargos por mora."));

        if (context.DraftSalesInvoicesCount > 0)
            anomalies.Add(new ERPAnomalyDto(
                "Facturas venta sin contabilizar", "Media",
                $"{context.DraftSalesInvoicesCount} factura(s) de venta en borrador pendientes de contabilizar.",
                "Revisar y contabilizar las facturas para generar vencimientos de cobro."));

        if (context.DraftPurchaseInvoicesCount > 0)
            anomalies.Add(new ERPAnomalyDto(
                "Facturas compra sin contabilizar", "Media",
                $"{context.DraftPurchaseInvoicesCount} factura(s) de compra en borrador pendientes de contabilizar.",
                "Revisar y contabilizar para registrar las deudas con proveedores."));

        var salesBalance = context.TotalSalesThisMonth - context.TotalPurchasesThisMonth;
        if (context.TotalPurchasesThisMonth > context.TotalSalesThisMonth && context.TotalSalesThisMonth > 0)
            anomalies.Add(new ERPAnomalyDto(
                "Compras superan ventas este mes", "Info",
                $"Ventas: {context.TotalSalesThisMonth:N2} € | Compras: {context.TotalPurchasesThisMonth:N2} € | Saldo: {salesBalance:N2} €",
                "Revisar si el patrón es estacional o requiere atención."));

        if (!anomalies.Any())
            anomalies.Add(new ERPAnomalyDto(
                "Sin anomalías detectadas", "Ok",
                "Todos los indicadores ERP están dentro de los parámetros normales.", null));

        return anomalies;
    }
}
