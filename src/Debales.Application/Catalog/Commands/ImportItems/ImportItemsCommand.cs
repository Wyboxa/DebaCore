namespace Debales.Application.Catalog.Commands.ImportItems;

public sealed record ImportItemsCommand(string CsvContent, string CreatedBy);
