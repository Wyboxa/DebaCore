namespace Debales.Application.CRM.Customers.Commands.ImportCustomers;

public sealed record ImportCustomersCommand(string CsvContent, string CreatedBy);
