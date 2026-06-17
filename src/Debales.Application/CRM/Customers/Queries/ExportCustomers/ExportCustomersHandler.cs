using Debales.Application.Common.Export;

namespace Debales.Application.CRM.Customers.Queries.ExportCustomers;

public sealed class ExportCustomersHandler(ICustomerRepository customers)
{
    private static readonly IReadOnlyList<string> Headers =
        ["Nombre", "Sector", "CIF", "Teléfono", "Email", "Cuenta contable", "Activo", "Creado"];

    public async Task<byte[]> Handle(CancellationToken ct = default)
    {
        var list = await customers.GetAllAsync(ct);
        var rows = list.Select(c => (IReadOnlyList<object?>)
        [
            c.Name,
            c.Sector,
            c.TaxId,
            c.Phone,
            c.Email,
            c.AccountCode,
            !c.IsDeleted,
            c.CreatedAt
        ]);
        return ExcelExporter.Generate("Clientes", Headers, rows);
    }
}
