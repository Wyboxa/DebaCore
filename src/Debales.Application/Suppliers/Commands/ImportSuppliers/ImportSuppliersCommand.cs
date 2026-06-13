namespace Debales.Application.Suppliers.Commands.ImportSuppliers;

public sealed record ImportSuppliersCommand(string CsvContent, string CreatedBy);
