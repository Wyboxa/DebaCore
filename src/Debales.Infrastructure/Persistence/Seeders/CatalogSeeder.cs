using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Seeders;

public static class CatalogSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedUnitsOfMeasureAsync(context);
        await SeedTaxTypesAsync(context);
        await SeedItemFamiliesAsync(context);
    }

    private static async Task SeedUnitsOfMeasureAsync(ApplicationDbContext context)
    {
        if (await context.UnitsOfMeasure.AnyAsync()) return;

        var units = new[]
        {
            UnitOfMeasure.Create("UN",  "Unidad",           "seed"),
            UnitOfMeasure.Create("KG",  "Kilogramo",        "seed"),
            UnitOfMeasure.Create("G",   "Gramo",            "seed"),
            UnitOfMeasure.Create("L",   "Litro",            "seed"),
            UnitOfMeasure.Create("ML",  "Mililitro",        "seed"),
            UnitOfMeasure.Create("M",   "Metro",            "seed"),
            UnitOfMeasure.Create("M2",  "Metro cuadrado",   "seed"),
            UnitOfMeasure.Create("M3",  "Metro cúbico",     "seed"),
            UnitOfMeasure.Create("H",   "Hora",             "seed"),
            UnitOfMeasure.Create("DIA", "Día",              "seed"),
        };

        await context.UnitsOfMeasure.AddRangeAsync(units);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTaxTypesAsync(ApplicationDbContext context)
    {
        if (await context.TaxTypes.AnyAsync()) return;

        var taxTypes = new[]
        {
            TaxType.Create("IVA21",  "IVA General (21%)",        21m,  "seed"),
            TaxType.Create("IVA10",  "IVA Reducido (10%)",       10m,  "seed"),
            TaxType.Create("IVA4",   "IVA Superreducido (4%)",   4m,   "seed"),
            TaxType.Create("EXENTO", "Exento de IVA",            0m,   "seed"),
        };

        await context.TaxTypes.AddRangeAsync(taxTypes);
        await context.SaveChangesAsync();
    }

    private static async Task SeedItemFamiliesAsync(ApplicationDbContext context)
    {
        if (await context.ItemFamilies.AnyAsync()) return;

        var families = new[]
        {
            ItemFamily.Create("GEN",   "General",     "seed"),
            ItemFamily.Create("PROD",  "Productos",   "seed"),
            ItemFamily.Create("SERV",  "Servicios",   "seed"),
            ItemFamily.Create("MANT",  "Mantenimiento","seed"),
        };

        await context.ItemFamilies.AddRangeAsync(families);
        await context.SaveChangesAsync();
    }
}
