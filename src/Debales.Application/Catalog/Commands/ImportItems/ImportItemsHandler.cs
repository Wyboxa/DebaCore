using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.ImportItems;

/// <summary>
/// Importa artículos desde CSV con columnas:
/// Código,Nombre,Descripción,Servicio,UnidadMedida,PrecioVenta,PrecioCompra,StockMínimo
///
/// UnidadMedida: código o nombre del UoM existente (ej. "UND", "KG", "HOR").
/// Si no se reconoce, se usa la primera unidad de medida disponible.
/// </summary>
public sealed class ImportItemsHandler
{
    private readonly IItemRepository _items;
    private readonly IUnitOfMeasureRepository _uoms;
    private readonly IItemFamilyRepository _families;
    private readonly ITaxTypeRepository _taxTypes;
    private readonly IUnitOfWork _unitOfWork;

    public ImportItemsHandler(
        IItemRepository items,
        IUnitOfMeasureRepository uoms,
        IItemFamilyRepository families,
        ITaxTypeRepository taxTypes,
        IUnitOfWork unitOfWork)
    {
        _items = items;
        _uoms = uoms;
        _families = families;
        _taxTypes = taxTypes;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImportResult> Handle(ImportItemsCommand command, CancellationToken ct = default)
    {
        var allUoms = await _uoms.GetAllActiveAsync(ct);
        var allFamilies = await _families.GetAllAsync(ct);
        var allTaxTypes = await _taxTypes.GetAllAsync(ct);

        var defaultUom = allUoms.FirstOrDefault()
            ?? throw new InvalidOperationException("No hay unidades de medida configuradas. Cree al menos una antes de importar artículos.");

        var rows = CsvParser.Parse(command.CsvContent);
        int success = 0, failed = 0;
        var errors = new List<string>();

        foreach (var (row, lineNum) in rows.Select((r, i) => (r, i + 2)))
        {
            var code = CsvParser.Field(row, 0);
            var name = CsvParser.Field(row, 1);

            if (code is null || name is null)
            {
                errors.Add($"Fila {lineNum}: Código y Nombre son obligatorios.");
                failed++;
                continue;
            }

            try
            {
                var uomName = CsvParser.Field(row, 4)?.ToUpperInvariant();
                var uom = uomName is not null
                    ? allUoms.FirstOrDefault(u =>
                        u.Code.Equals(uomName, StringComparison.OrdinalIgnoreCase) ||
                        u.Name.Equals(uomName, StringComparison.OrdinalIgnoreCase))
                      ?? defaultUom
                    : defaultUom;

                var familyName = CsvParser.Field(row, 5);
                var family = familyName is not null
                    ? allFamilies.FirstOrDefault(f =>
                        f.Name.Equals(familyName, StringComparison.OrdinalIgnoreCase))
                    : null;

                var taxName = CsvParser.Field(row, 6);
                var taxType = taxName is not null
                    ? allTaxTypes.FirstOrDefault(t =>
                        t.Code.Equals(taxName, StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals(taxName, StringComparison.OrdinalIgnoreCase) ||
                        t.Rate.ToString("F0").Equals(taxName.TrimEnd('%').Trim(), StringComparison.OrdinalIgnoreCase))
                    : allTaxTypes.FirstOrDefault();

                var salePrice   = CsvParser.ParseDecimal(row, 7);
                var purchasePrice = CsvParser.ParseDecimal(row, 8);
                var minStock    = CsvParser.ParseDecimal(row, 9, -1);
                var isService   = CsvParser.ParseBool(row, 3);

                var item = Item.Create(
                    code, name,
                    CsvParser.Field(row, 2),  // Descripción
                    isService,
                    uom.Id,
                    family?.Id,
                    taxType?.Id,
                    salePrice,
                    purchasePrice,
                    command.CreatedBy,
                    minStock >= 0 ? minStock : null);

                await _items.AddAsync(item, ct);
                success++;
            }
            catch (Exception ex)
            {
                errors.Add($"Fila {lineNum} ({code}): {ex.Message}");
                failed++;
            }
        }

        if (success > 0)
            await _unitOfWork.SaveChangesAsync(ct);

        return new ImportResult(success, failed, errors);
    }
}
