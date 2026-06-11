using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetRemittanceById;

public sealed class GetRemittanceByIdHandler(IRemittanceRepository remittances)
{
    public async Task<RemittanceDetailDto?> Handle(GetRemittanceByIdQuery query, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(query.Id, ct);
        return remittance is null ? null : UpdateRemittanceHandler.ToDto(remittance);
    }
}
