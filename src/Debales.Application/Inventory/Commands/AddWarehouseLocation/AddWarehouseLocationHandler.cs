using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.AddWarehouseLocation;

public sealed class AddWarehouseLocationHandler
{
    private readonly IWarehouseLocationRepository _locations;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public AddWarehouseLocationHandler(IWarehouseLocationRepository locations, IWarehouseRepository warehouses, IUnitOfWork uow)
    {
        _locations = locations;
        _warehouses = warehouses;
        _uow = uow;
    }

    public async Task<WarehouseLocationDto> Handle(AddWarehouseLocationCommand command, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouses.GetByIdAsync(command.WarehouseId, cancellationToken)
            ?? throw new InvalidOperationException($"Almacén '{command.WarehouseId}' no encontrado.");

        if (await _locations.ExistsByCodeAsync(command.WarehouseId, command.Code, cancellationToken))
            throw new InvalidOperationException($"Ya existe la ubicación '{command.Code}' en este almacén.");

        var location = WarehouseLocation.Create(command.WarehouseId, command.Code, command.Description, command.CreatedBy);
        await _locations.AddAsync(location, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new WarehouseLocationDto(location.Id, location.WarehouseId, location.Code, location.Description, location.IsActive);
    }
}
