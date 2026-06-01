using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetWarehouseById;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.CreateWarehouse;

public sealed class CreateWarehouseHandler
{
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public CreateWarehouseHandler(IWarehouseRepository warehouses, IUnitOfWork uow)
    {
        _warehouses = warehouses;
        _uow = uow;
    }

    public async Task<WarehouseDetailDto> Handle(CreateWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        if (await _warehouses.ExistsByCodeAsync(command.Code, cancellationToken))
            throw new InvalidOperationException($"Ya existe un almacén con el código '{command.Code}'.");

        var warehouse = Warehouse.Create(command.Code, command.Name, command.Description, command.CreatedBy);
        await _warehouses.AddAsync(warehouse, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _warehouses.GetByIdAsync(warehouse.Id, cancellationToken);
        return GetWarehouseByIdHandler.ToDetailDto(saved!);
    }
}
