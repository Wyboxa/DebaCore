using Debales.Application.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Application.CRM.Customers.Commands.ImportCustomers;

/// <summary>
/// Importa clientes desde CSV con columnas:
/// Nombre,Sector,CIF,Teléfono,Email
/// </summary>
public sealed class ImportCustomersHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public ImportCustomersHandler(ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImportResult> Handle(ImportCustomersCommand command, CancellationToken ct = default)
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
                var customer = Customer.Create(
                    name,
                    CsvParser.Field(row, 1),  // Sector
                    CsvParser.Field(row, 2),  // CIF
                    CsvParser.Field(row, 3),  // Teléfono
                    CsvParser.Field(row, 4),  // Email
                    command.CreatedBy);

                await _customers.AddAsync(customer, ct);
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
