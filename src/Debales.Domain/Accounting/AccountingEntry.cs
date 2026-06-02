using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class AccountingEntry : AuditableEntity
{
    private readonly List<AccountingEntryLine> _lines = [];
    private AccountingEntry() { }

    public string Number { get; private set; } = null!;
    public DateOnly Date { get; private set; }
    public string Description { get; private set; } = null!;
    public Guid JournalId { get; private set; }
    public Guid FiscalPeriodId { get; private set; }
    public EntryStatus Status { get; private set; } = EntryStatus.Draft;
    public string? SourceType { get; private set; }
    public Guid? SourceId { get; private set; }

    public AccountingJournal? Journal { get; private set; }
    public FiscalPeriod? FiscalPeriod { get; private set; }

    public IReadOnlyList<AccountingEntryLine> Lines => _lines.AsReadOnly();

    public decimal TotalDebit => _lines.Sum(l => l.Debit);
    public decimal TotalCredit => _lines.Sum(l => l.Credit);
    public bool IsBalanced => TotalDebit == TotalCredit;

    public static AccountingEntry Create(
        string number, DateOnly date, string description,
        Guid journalId, Guid fiscalPeriodId,
        string? sourceType, Guid? sourceId, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        if (journalId == Guid.Empty) throw new ArgumentException("El diario es obligatorio.");
        if (fiscalPeriodId == Guid.Empty) throw new ArgumentException("El período fiscal es obligatorio.");

        return new AccountingEntry
        {
            Number = number.Trim(),
            Date = date,
            Description = description.Trim(),
            JournalId = journalId,
            FiscalPeriodId = fiscalPeriodId,
            SourceType = sourceType,
            SourceId = sourceId,
            CreatedBy = createdBy
        };
    }

    public AccountingEntryLine AddLine(
        Guid accountId, string accountCode, string description,
        decimal debit, decimal credit,
        Guid? thirdPartyId, string? thirdPartyType)
    {
        if (Status != EntryStatus.Draft)
            throw new InvalidOperationException("Solo se pueden modificar líneas en asientos en borrador.");
        if (debit < 0 || credit < 0)
            throw new ArgumentException("Los importes no pueden ser negativos.");
        if (debit > 0 && credit > 0)
            throw new ArgumentException("Una línea no puede tener debe y haber simultáneamente.");

        var line = AccountingEntryLine.Create(
            Id, _lines.Count + 1,
            accountId, accountCode, description,
            debit, credit, thirdPartyId, thirdPartyType);
        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != EntryStatus.Draft)
            throw new InvalidOperationException("Solo se pueden contabilizar asientos en borrador.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("No se puede contabilizar un asiento sin líneas.");
        if (!IsBalanced)
            throw new InvalidOperationException($"El asiento no está cuadrado. Debe: {TotalDebit:N2}, Haber: {TotalCredit:N2}");

        Status = EntryStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == EntryStatus.Posted)
            throw new InvalidOperationException("Los asientos contabilizados no se pueden anular directamente. Crea un asiento de reversión.");
        if (Status == EntryStatus.Cancelled)
            throw new InvalidOperationException("El asiento ya está anulado.");

        Status = EntryStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
