using Debales.Domain.Catalog;
using Debales.Domain.CRM.Customers;
using Debales.Domain.Inventory;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence;

/// <summary>
/// Seeds realistic connected demo data for development and demonstration.
/// Idempotent: uses a well-known customer name as sentinel.
/// Requires CatalogSeeder to have run first (UoM, TaxTypes, ItemFamilies).
/// </summary>
public static class DemoDataSeeder
{
    private const string DemoMarker = "Construcciones Herrera S.L.";

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Customers.AnyAsync(c => c.Name == DemoMarker))
            return;

        // --- Asegura catálogo base (por si no se llamó a CatalogSeeder) ---
        await EnsureCatalogAsync(context);

        // --- Lookup entidades de catálogo ---
        var uomUnit = await context.UnitsOfMeasure.FirstOrDefaultAsync(u => u.Code == "UN")
            ?? await context.UnitsOfMeasure.FirstAsync();
        var uomHour = await context.UnitsOfMeasure.FirstOrDefaultAsync(u => u.Code == "H")
            ?? uomUnit;
        var uomKg = await context.UnitsOfMeasure.FirstOrDefaultAsync(u => u.Code == "KG")
            ?? uomUnit;
        var tax21 = await context.TaxTypes.FirstOrDefaultAsync(t => t.Code == "IVA21")
            ?? await context.TaxTypes.FirstAsync();
        var tax10 = await context.TaxTypes.FirstOrDefaultAsync(t => t.Code == "IVA10")
            ?? tax21;
        var famProd = await context.ItemFamilies.FirstOrDefaultAsync(f => f.Code == "PROD")
            ?? await context.ItemFamilies.FirstAsync();
        var famServ = await context.ItemFamilies.FirstOrDefaultAsync(f => f.Code == "SERV")
            ?? famProd;

        // --- Artículos ---
        var itemTubo = Item.Create("TUB-001", "Tubo acero galvanizado DN50",
            "Tubo acero galvanizado diámetro 50mm",
            false, uomUnit.Id, famProd.Id, tax21.Id, 18.50m, 12.00m, "seed");
        var itemValvula = Item.Create("VAL-001", "Válvula de corte DN50",
            "Válvula de corte esférica DN50 PN16",
            false, uomUnit.Id, famProd.Id, tax21.Id, 45.00m, 28.00m, "seed");
        var itemCemento = Item.Create("CEM-001", "Cemento Portland 25kg",
            "Saco cemento Portland CEM II/B-M 25kg",
            false, uomKg.Id, famProd.Id, tax10.Id, 6.80m, 4.20m, "seed");
        var itemInstalacion = Item.Create("SVC-INS", "Instalación hidráulica",
            "Servicio de instalación hidráulica (por hora)",
            true, uomHour.Id, famServ.Id, tax21.Id, 55.00m, 0m, "seed");
        var itemMantenimiento = Item.Create("SVC-MNT", "Mantenimiento preventivo",
            "Revisión y mantenimiento preventivo de instalación",
            true, uomHour.Id, famServ.Id, tax21.Id, 48.00m, 0m, "seed");
        context.Items.AddRange(itemTubo, itemValvula, itemCemento, itemInstalacion, itemMantenimiento);

        // --- Clientes ---
        var clienteHerrera = Customer.Create(
            "Construcciones Herrera S.L.", "Construcción",
            "B12345678", "+34 955 123 456", "compras@herrera-sl.es", "seed");
        var clienteNorte = Customer.Create(
            "Servicios Norte S.A.", "Mantenimiento",
            "A87654321", "+34 943 987 654", "administracion@serviciosnorte.com", "seed");
        var clientePerez = Customer.Create(
            "Talleres Pérez e Hijos", "Industria",
            "B99887766", "+34 971 654 321", "info@talleresperez.es", "seed");
        context.Customers.AddRange(clienteHerrera, clienteNorte, clientePerez);

        // --- Almacén principal ---
        var almacen = Warehouse.Create("ALM-01", "Almacén Principal Sevilla",
            "Almacén central — Polígono Industrial Norte, Sevilla", "seed");
        context.Warehouses.Add(almacen);

        await context.SaveChangesAsync();

        // --- Stock inicial ---
        AddStock(context, "MOV-2026-0001", itemTubo, almacen, 500m);
        AddStock(context, "MOV-2026-0002", itemValvula, almacen, 150m);
        AddStock(context, "MOV-2026-0003", itemCemento, almacen, 2000m);
        await context.SaveChangesAsync();

        // =====================================================================
        // ESCENARIO A — Pedido Confirmado, pendiente de generar albarán
        // =====================================================================
        var pedidoA = SalesOrder.Create(
            "PV-2026-0001", clienteHerrera.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-15)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
            "Obra calle Betis, 34 — Fase 2", "seed");
        pedidoA.AddLine(itemTubo.Id, itemTubo.Code, itemTubo.Name, null, 50m, 18.50m, 21m);
        pedidoA.AddLine(itemValvula.Id, itemValvula.Code, itemValvula.Name, null, 10m, 45.00m, 21m);
        pedidoA.AddLine(itemInstalacion.Id, itemInstalacion.Code, itemInstalacion.Name, null, 8m, 55.00m, 21m);
        pedidoA.Confirm("seed");
        context.SalesOrders.Add(pedidoA);

        // =====================================================================
        // ESCENARIO B — Pedido → Albarán Emitido, pendiente de facturar
        // =====================================================================
        var pedidoB = SalesOrder.Create(
            "PV-2026-0002", clienteNorte.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-20)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            "Mantenimiento trimestral instalaciones", "seed");
        pedidoB.AddLine(itemMantenimiento.Id, itemMantenimiento.Code, itemMantenimiento.Name, null, 16m, 48.00m, 21m);
        pedidoB.AddLine(itemValvula.Id, itemValvula.Code, itemValvula.Name,
            "Sustitución válvulas defectuosas", 3m, 45.00m, 21m);
        pedidoB.Confirm("seed");
        context.SalesOrders.Add(pedidoB);
        await context.SaveChangesAsync();

        var albaranB = SalesDeliveryNote.Create(
            "ALV-2026-0001", pedidoB.CustomerId, pedidoB.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),
            "Entrega completa según pedido PV-2026-0002", "seed");
        foreach (var ln in pedidoB.Lines)
            albaranB.AddLine(ln.Id, pedidoB.Id, ln.ItemId, ln.ItemCode, ln.ItemName, ln.Description, ln.Quantity);
        albaranB.Post("seed");
        context.SalesDeliveryNotes.Add(albaranB);

        // =====================================================================
        // ESCENARIO C — Flujo completo: Pedido → Albarán (emitido) → Factura (emitida)
        // =====================================================================
        var pedidoC = SalesOrder.Create(
            "PV-2026-0003", clientePerez.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-45)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-30)),
            "Suministro materiales Fase 1", "seed");
        pedidoC.AddLine(itemTubo.Id, itemTubo.Code, itemTubo.Name, null, 100m, 18.50m, 21m);
        pedidoC.AddLine(itemCemento.Id, itemCemento.Code, itemCemento.Name, null, 500m, 6.80m, 10m);
        pedidoC.Confirm("seed");
        context.SalesOrders.Add(pedidoC);
        await context.SaveChangesAsync();

        var albaranC = SalesDeliveryNote.Create(
            "ALV-2026-0002", pedidoC.CustomerId, pedidoC.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-40)),
            "Entrega completa Fase 1", "seed");
        foreach (var ln in pedidoC.Lines)
            albaranC.AddLine(ln.Id, pedidoC.Id, ln.ItemId, ln.ItemCode, ln.ItemName, ln.Description, ln.Quantity);
        albaranC.Post("seed");
        context.SalesDeliveryNotes.Add(albaranC);
        await context.SaveChangesAsync();

        var facturaC = SalesInvoice.Create(
            "FV-2026-0001", pedidoC.CustomerId, albaranC.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-38)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            null, "seed");
        foreach (var ln in pedidoC.Lines)
            facturaC.AddLine(ln.ItemId, ln.ItemCode, ln.ItemName, ln.Description, ln.Quantity, ln.UnitPrice, ln.TaxRate);
        facturaC.Post("seed");
        context.SalesInvoices.Add(facturaC);

        await context.SaveChangesAsync();
    }

    private static void AddStock(ApplicationDbContext context, string number, Item item, Warehouse warehouse, decimal qty)
    {
        var mov = StockMovement.Create(number, StockMovementType.In,
            item.Id, item.Code, item.Name,
            warehouse.Id, null,
            DateOnly.FromDateTime(DateTime.Today.AddDays(-30)),
            qty, "Apertura demo", "Stock inicial demo", "seed");
        var bal = StockBalance.Create(item.Id, warehouse.Id);
        bal.Apply(qty);
        context.StockMovements.Add(mov);
        context.StockBalances.Add(bal);
    }

    private static async Task EnsureCatalogAsync(ApplicationDbContext context)
    {
        if (!await context.UnitsOfMeasure.AnyAsync())
        {
            context.UnitsOfMeasure.AddRange(
                UnitOfMeasure.Create("UN", "Unidad", "seed"),
                UnitOfMeasure.Create("KG", "Kilogramo", "seed"),
                UnitOfMeasure.Create("H", "Hora", "seed"));
            await context.SaveChangesAsync();
        }

        if (!await context.TaxTypes.AnyAsync())
        {
            context.TaxTypes.AddRange(
                TaxType.Create("IVA21", "IVA General (21%)", 21m, "seed"),
                TaxType.Create("IVA10", "IVA Reducido (10%)", 10m, "seed"));
            await context.SaveChangesAsync();
        }

        if (!await context.ItemFamilies.AnyAsync())
        {
            context.ItemFamilies.AddRange(
                ItemFamily.Create("PROD", "Productos", "seed"),
                ItemFamily.Create("SERV", "Servicios", "seed"));
            await context.SaveChangesAsync();
        }
    }
}
