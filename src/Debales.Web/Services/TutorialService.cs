using Microsoft.JSInterop;

namespace Debales.Web.Services;

public sealed record TutorialStep(
    string Title,
    string Body,
    string? NavigateTo = null);

public sealed class TutorialService
{
    private const string KeyEnabled = "debales_tutorial_enabled";
    private const string KeyStep = "debales_tutorial_step";
    private const string KeyActive = "debales_tutorial_active";

    private readonly IJSRuntime _js;

    public bool IsEnabled { get; private set; }
    public bool IsActive { get; private set; }
    public int CurrentStep { get; private set; }

    public event Action? OnChange;

    public static readonly IReadOnlyList<TutorialStep> Steps =
    [
        new("Bienvenido a Debales",
            "Esta es tu plataforma CRM/ERP. El menú izquierdo te da acceso a todos los módulos. Navega entre secciones y usa este tutorial para conocer cada área."),

        new("CRM — Clientes",
            "En la sección CRM gestionas tus clientes. Puedes registrar contactos, anotar actividades, añadir notas y hacer seguimiento de oportunidades de venta.",
            "/crm/customers"),

        new("Catálogo de artículos",
            "Define los artículos y servicios que vendes. Cada artículo tiene familia, unidad de medida, precio e IVA. Son la base de presupuestos, pedidos y facturas.",
            "/catalogo"),

        new("Flujo de ventas",
            "El ciclo completo de venta es: Presupuesto → Pedido → Albarán → Factura. Cada paso convierte el anterior con un solo clic. Las facturas generan automáticamente el asiento contable y el vencimiento de cobro.",
            "/ventas/presupuestos"),

        new("Flujo de compras",
            "El ciclo de compra es: Pedido a proveedor → Albarán de entrada → Factura de compra. Al confirmar el albarán se actualiza el stock automáticamente.",
            "/compras/pedidos"),

        new("Inventario",
            "Los movimientos de stock se generan solos al confirmar albaranes de venta y compra. Aquí puedes consultar saldos por almacén y el historial de movimientos.",
            "/inventario/movimientos"),

        new("Contabilidad",
            "Los asientos contables se crean automáticamente al contabilizar facturas y registrar cobros o pagos. El plan contable PGC España ya está preconfigurado.",
            "/contabilidad/asientos"),

        new("Configuración",
            "Desde Configuración gestionas usuarios, roles, licencia y este tutorial. Puedes volver a activarlo cuando quieras desde la tarjeta 'Tutorial'.",
            "/configuracion"),
    ];

    public int TotalSteps => Steps.Count;
    public TutorialStep Current => Steps[Math.Clamp(CurrentStep, 0, Steps.Count - 1)];
    public bool IsFirst => CurrentStep == 0;
    public bool IsLast => CurrentStep == Steps.Count - 1;

    public TutorialService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task LoadAsync()
    {
        try
        {
            var enabled = await _js.InvokeAsync<string?>("localStorage.getItem", KeyEnabled);
            var active = await _js.InvokeAsync<string?>("localStorage.getItem", KeyActive);
            var step = await _js.InvokeAsync<string?>("localStorage.getItem", KeyStep);

            IsEnabled = enabled != "false";
            IsActive = active == "true";
            CurrentStep = int.TryParse(step, out var s) ? Math.Clamp(s, 0, Steps.Count - 1) : 0;
        }
        catch
        {
            IsEnabled = true;
            IsActive = false;
            CurrentStep = 0;
        }
    }

    public async Task StartAsync()
    {
        IsActive = true;
        CurrentStep = 0;
        await PersistAsync();
        Notify();
    }

    public async Task NextAsync()
    {
        if (IsLast)
        {
            await FinishAsync();
            return;
        }
        CurrentStep++;
        await PersistAsync();
        Notify();
    }

    public async Task PreviousAsync()
    {
        if (CurrentStep > 0) CurrentStep--;
        await PersistAsync();
        Notify();
    }

    public async Task SkipAsync()
    {
        await FinishAsync();
    }

    public async Task FinishAsync()
    {
        IsActive = false;
        CurrentStep = 0;
        await PersistAsync();
        Notify();
    }

    public async Task SetEnabledAsync(bool enabled)
    {
        IsEnabled = enabled;
        if (!enabled) IsActive = false;
        await PersistAsync();
        Notify();
    }

    public async Task ResetAsync()
    {
        IsEnabled = true;
        IsActive = false;
        CurrentStep = 0;
        await PersistAsync();
        Notify();
    }

    private async Task PersistAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", KeyEnabled, IsEnabled.ToString().ToLower());
            await _js.InvokeVoidAsync("localStorage.setItem", KeyActive, IsActive.ToString().ToLower());
            await _js.InvokeVoidAsync("localStorage.setItem", KeyStep, CurrentStep.ToString());
        }
        catch { }
    }

    private void Notify() => OnChange?.Invoke();
}
