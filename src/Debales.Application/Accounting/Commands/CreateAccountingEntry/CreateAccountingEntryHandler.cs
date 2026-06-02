using Debales.Application.Accounting.DTOs;
using Debales.Application.Accounting.Queries.GetAccountingEntryById;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateAccountingEntry;

public sealed class CreateAccountingEntryHandler
{
    private readonly IAccountingEntryRepository _entries;
    private readonly IAccountingJournalRepository _journals;
    private readonly IUnitOfWork _uow;

    public CreateAccountingEntryHandler(
        IAccountingEntryRepository entries,
        IAccountingJournalRepository journals,
        IUnitOfWork uow)
    {
        _entries = entries;
        _journals = journals;
        _uow = uow;
    }

    public async Task<AccountingEntryDetailDto> Handle(CreateAccountingEntryCommand command, CancellationToken ct = default)
    {
        var journal = await _journals.GetByIdAsync(command.JournalId, ct)
            ?? throw new InvalidOperationException($"Diario '{command.JournalId}' no encontrado.");

        var number = await _entries.GetNextNumberAsync(journal.Id, journal.Code, ct);
        var entry = AccountingEntry.Create(
            number, command.Date, command.Description,
            command.JournalId, command.FiscalPeriodId,
            null, null, command.CreatedBy);

        foreach (var l in command.Lines)
            entry.AddLine(l.AccountId, l.AccountCode, l.Description, l.Debit, l.Credit, l.ThirdPartyId, l.ThirdPartyType);

        await _entries.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);

        var saved = await _entries.GetByIdWithLinesAsync(entry.Id, ct);
        return GetAccountingEntryByIdHandler.ToDto(saved!);
    }
}
