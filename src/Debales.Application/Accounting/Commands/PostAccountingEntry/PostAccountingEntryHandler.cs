using Debales.Application.Accounting.DTOs;
using Debales.Application.Accounting.Queries.GetAccountingEntryById;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.PostAccountingEntry;

public sealed class PostAccountingEntryHandler
{
    private readonly IAccountingEntryRepository _entries;
    private readonly IUnitOfWork _uow;

    public PostAccountingEntryHandler(IAccountingEntryRepository entries, IUnitOfWork uow)
    {
        _entries = entries;
        _uow = uow;
    }

    public async Task<AccountingEntryDetailDto> Handle(PostAccountingEntryCommand command, CancellationToken ct = default)
    {
        var entry = await _entries.GetByIdWithLinesAsync(command.Id, ct)
            ?? throw new InvalidOperationException($"Asiento '{command.Id}' no encontrado.");

        entry.Post(command.UpdatedBy);
        await _uow.SaveChangesAsync(ct);

        var saved = await _entries.GetByIdWithLinesAsync(entry.Id, ct);
        return GetAccountingEntryByIdHandler.ToDto(saved!);
    }
}
