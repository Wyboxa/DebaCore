using Microsoft.JSInterop;

namespace Debales.Web.Services;

public sealed record TutorialStep(
    string Title,
    string Body,
    string? NavigateTo = null,
    string[]? Tips = null);

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
            "El Dashboard muestra los indicadores clave de tu empresa en tiempo real. Los KPIs te alertan de situaciones que requieren atención inmediata antes de que se conviertan en problemas.",
            "/",
            ["Cobros y pagos vencidos aparecen destacados en rojo",
             "Stock por debajo del mínimo configurable genera una alerta",
             "La posición de tesorería suma bancos y cajas en tiempo real"]),

        new("CRM — Clientes",
            "Cada ficha de cliente agrupa toda su información comercial y financiera: contactos, actividades, oportunidades, historial de pedidos y estado de cuenta, todo en un solo lugar.",
            "/crm/customers",
            ["Tab Pedidos: historial de pedidos de venta con estado y navegación directa",
             "Tab Cuenta: estado de cuenta con saldo acumulado por fecha",
             "Asigna tarifa de precio y condición de pago por defecto al cliente",
             "Tab Asistente: consulta el resumen del cliente con IA"]),

        new("Proveedores",
            "Los proveedores funcionan de forma simétrica a los clientes. Cada ficha incluye toda la trazabilidad de compras y la relación financiera con el proveedor.",
            "/proveedores",
            ["Tab Pedidos: historial de pedidos de compra del proveedor",
             "Tab Cuenta: estado de cuenta con saldo acumulado",
             "Tab Códigos: códigos de artículo propios del proveedor para sus albaranes"]),

        new("Catálogo — Artículos y tarifas",
            "Define los artículos y servicios con sus precios, IVA y familia. Las Tarifas permiten precios especiales que se aplican automáticamente según el cliente.",
            "/catalogo",
            ["La ficha de artículo tiene tabs: Movimientos de stock con saldo acumulado y Saldos por almacén",
             "Configura stock mínimo por artículo para recibir alertas de reposición",
             "Las tarifas se resuelven automáticamente al añadir artículos en pedidos y presupuestos"]),

        new("Ventas — del presupuesto a la factura",
            "El ciclo completo de venta sigue siempre el mismo orden: Presupuesto → Pedido → Albarán → Factura. Cada paso convierte el anterior con un solo clic, sin reintroducir datos.",
            "/ventas/presupuestos",
            ["El presupuesto pasa por estados: Borrador → Enviado → Aceptado → Convertido a pedido",
             "El albarán confirmado descuenta el stock automáticamente",
             "La factura contabilizada genera el asiento contable y el vencimiento de cobro",
             "El precio en pedido se resuelve desde la tarifa asignada al cliente"]),

        new("Compras — del pedido a la factura",
            "El ciclo de compra sigue el mismo patrón que ventas: Pedido → Albarán → Factura. Todo simétrico para facilitar el aprendizaje y el control.",
            "/compras/pedidos",
            ["El albarán de entrada actualiza el stock disponible al confirmarlo",
             "El estado del pedido se actualiza solo al recibir albaranes parciales",
             "La factura de compra genera el asiento contable y el vencimiento de pago"]),

        new("Inventario — stock en tiempo real",
            "Los movimientos de stock se crean automáticamente al confirmar albaranes de venta y compra. No necesitas registrarlos a mano; el sistema los genera y los trazabiliza.",
            "/inventario/movimientos",
            ["Saldos: consulta stock actual por artículo y almacén",
             "Badge rojo 'Bajo mínimo' cuando el stock cae por debajo del umbral configurado",
             "Ajuste rápido directamente desde la pantalla de saldos"]),

        new("Recuento físico de inventario",
            "El Recuento de inventario te permite realizar un inventario físico y reconciliarlo con el stock del sistema, generando los ajustes de forma automática para las diferencias.",
            "/inventario/conteo",
            ["Crea una sesión de recuento y añade los artículos que quieres contar",
             "Registra las cantidades contadas artículo a artículo desde la ficha del recuento",
             "Al cerrar, el sistema compara y genera los movimientos de ajuste necesarios"]),

        new("Contabilidad — asientos automáticos",
            "Los asientos se generan solos al contabilizar facturas y registrar cobros o pagos. El plan contable PGC España ya está preconfigurado y listo para usar.",
            "/contabilidad/asientos",
            ["Los asientos no pueden modificarse una vez contabilizados, solo anularse con trazabilidad",
             "Cada asiento referencia el documento que lo originó",
             "Informes disponibles: Balance de comprobación, Libro diario y Balance de situación"]),

        new("Tesorería y remesas bancarias",
            "La pantalla de Tesorería agrupa los saldos de todas tus cuentas bancarias y cajas en un solo vistazo, mostrando tu posición financiera real en todo momento.",
            "/contabilidad/tesoreria",
            ["Gestiona cuentas bancarias y cajas desde el menú Contabilidad",
             "Las Remesas agrupan cobros o pagos para liquidarlos en bloque (SEPA, transferencias)",
             "Al confirmar una remesa, se liquidan automáticamente los vencimientos incluidos"]),

        new("Vencimientos — control de cobros y pagos",
            "El informe de vencimientos aging muestra en qué estado están tus cobros y pagos, organizados por antigüedad para que priorices las gestiones más urgentes.",
            "/contabilidad/vencimientos",
            ["Bucket Corriente: documentos que aún no han vencido",
             "Buckets 1-30, 31-60, 61-90 y +90 días muestran el atraso acumulado",
             "Los vencimientos vencidos se destacan visualmente para actuar de inmediato",
             "El estado de cuenta de cada cliente o proveedor muestra el detalle completo"]),

        new("Configuración — personaliza la plataforma",
            "Desde Configuración personalizas el comportamiento de toda la plataforma. Aquí controlas usuarios, series de numeración y los parámetros de facturación.",
            "/configuracion",
            ["Series documentales: prefijo y contador para cada tipo (FV-, PC-, PV-...)",
             "Condiciones de pago: contado, 30 días, 30/60/90 días, etc.",
             "Formas de pago: efectivo, transferencia, tarjeta, domiciliación bancaria",
             "Puedes relanzar este tutorial en cualquier momento desde esta pantalla"]),
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
