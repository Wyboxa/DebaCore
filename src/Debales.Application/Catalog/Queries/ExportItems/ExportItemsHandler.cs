using Debales.Application.Common.Export;

namespace Debales.Application.Catalog.Queries.ExportItems;

public sealed class ExportItemsHandler(IItemRepository items)
{
    private static readonly IReadOnlyList<string> Headers =
        ["Código", "Nombre", "Descripción", "Servicio", "P. Venta", "P. Compra", "Stock mínimo", "Activo", "Creado"];

    public async Task<byte[]> Handle(CancellationToken ct = default)
    {
        var list = await items.GetAllAsync(ct);
        var rows = list.Select(i => (IReadOnlyList<object?>)
        [
            i.Code,
            i.Name,
            i.Description,
            i.IsService,
            i.SalePrice,
            i.PurchasePrice,
            i.MinimumStock,
            i.IsActive,
            i.CreatedAt
        ]);
        return ExcelExporter.Generate("Artículos", Headers, rows);
    }
}
