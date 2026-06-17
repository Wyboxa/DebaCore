using Debales.Application.Common.Export;

namespace Debales.Application.Suppliers.Queries.ExportSuppliers;

public sealed class ExportSuppliersHandler(ISupplierRepository suppliers)
{
    private static readonly IReadOnlyList<string> Headers =
        ["Nombre", "CIF", "Teléfono", "Email", "Contacto", "Cuenta contable", "Activo", "Creado"];

    public async Task<byte[]> Handle(CancellationToken ct = default)
    {
        var list = await suppliers.GetAllAsync(ct);
        var rows = list.Select(s => (IReadOnlyList<object?>)
        [
            s.Name,
            s.TaxId,
            s.Phone,
            s.Email,
            s.ContactName,
            s.AccountCode,
            !s.IsDeleted,
            s.CreatedAt
        ]);
        return ExcelExporter.Generate("Proveedores", Headers, rows);
    }
}
