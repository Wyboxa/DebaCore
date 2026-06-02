using Debales.Application.Accounting.Commands.CreateAccountingJournal;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetAccountingJournals;

public sealed class GetAccountingJournalsHandler
{
    private readonly IAccountingJournalRepository _journals;

    public GetAccountingJournalsHandler(IAccountingJournalRepository journals) => _journals = journals;

    public async Task<IReadOnlyList<AccountingJournalDto>> Handle(GetAccountingJournalsQuery query, CancellationToken ct = default)
    {
        var journals = await _journals.GetActiveAsync(ct);
        return journals.Select(CreateAccountingJournalHandler.ToDto).ToList();
    }
}
