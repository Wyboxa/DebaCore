using Debales.Application.Common;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Commands.ImportSuppliers;

/// <summary>
/// Importa proveedores desde CSV con columnas:
/// Nombre,CIF,Teléfono,Email,Contacto
/// </summary>
public sealed class ImportSuppliersHandler
{
    private readonly ISupplierRepository _suppliers;
    private readonly IUnitOfWork _unitOfWork;

    public ImportSuppliersHandler(ISupplierRepository suppliers, IUnitOfWork unitOfWork)
    {
        _suppliers = suppliers;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImportResult> Handle(ImportSuppliersCommand command, CancellationToken ct = default)
    {
        var rows = CsvParser.Parse(command.CsvContent);
        int success = 0, failed = 0;
        var errors = new List<string>();

        foreach (var (row, lineNum) in rows.Select((r, i) => (r, i + 2)))
        {
            var name = CsvParser.Field(row, 0);
            if (name is null)
            {
                errors.Add($"Fila {lineNum}: Nombre es obligatorio.");
                failed++;
                continue;
            }

            try
            {
                var supplier = Supplier.Create(
                    name,
                    CsvParser.Field(row, 1),  // CIF
                    CsvParser.Field(row, 2),  // Teléfono
                    CsvParser.Field(row, 3),  // Email
                    CsvParser.Field(row, 4),  // Contacto
                    command.CreatedBy);

                await _suppliers.AddAsync(supplier, ct);
                success++;
            }
            catch (Exception ex)
            {
                errors.Add($"Fila {lineNum} ({name}): {ex.Message}");
                failed++;
            }
        }

        if (success > 0)
            await _unitOfWork.SaveChangesAsync(ct);

        return new ImportResult(success, failed, errors);
    }
}
