using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateAccountingJournal;

public sealed class CreateAccountingJournalHandler
{
    private readonly IAccountingJournalRepository _journals;
    private readonly IUnitOfWork _uow;

    public CreateAccountingJournalHandler(IAccountingJournalRepository journals, IUnitOfWork uow)
    {
        _journals = journals;
        _uow = uow;
    }

    public async Task<AccountingJournalDto> Handle(CreateAccountingJournalCommand command, CancellationToken ct = default)
    {
        if (await _journals.ExistsByCodeAsync(command.Code, ct))
            throw new InvalidOperationException($"Ya existe un diario con el código '{command.Code}'.");

        var journal = AccountingJournal.Create(command.Code, command.Name, command.CreatedBy);
        await _journals.AddAsync(journal, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(journal);
    }

    internal static AccountingJournalDto ToDto(AccountingJournal j) =>
        new(j.Id, j.Code, j.Name, j.IsActive);
}
