using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class Remittance : AuditableEntity
{
    private readonly List<RemittanceLine> _lines = [];
    private Remittance() { }

    public string Number { get; private set; } = null!;
    public DateOnly Date { get; private set; }
    public Guid BankAccountId { get; private set; }
    public RemittanceType Type { get; private set; }
    public RemittanceStatus Status { get; private set; } = RemittanceStatus.Draft;
    public string? Notes { get; private set; }
    public string? FailReason { get; private set; }

    public BankAccount? BankAccount { get; private set; }
    public IReadOnlyList<RemittanceLine> Lines => _lines.AsReadOnly();

    public decimal Total => _lines.Sum(l => l.Amount);
    public int LineCount => _lines.Count;

    public static Remittance Create(string number, DateOnly date, Guid bankAccountId,
        RemittanceType type, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (bankAccountId == Guid.Empty) throw new ArgumentException("La cuenta bancaria es obligatoria.", nameof(bankAccountId));
        return new Remittance
        {
            Number      = number.Trim().ToUpperInvariant(),
            Date        = date,
            BankAccountId = bankAccountId,
            Type        = type,
            Status      = RemittanceStatus.Draft,
            Notes       = notes?.Trim(),
            CreatedBy   = createdBy
        };
    }

    public void UpdateNotes(string? notes, string updatedBy)
    {
        if (Status != RemittanceStatus.Draft)
            throw new InvalidOperationException("Solo se pueden editar remesas en estado Borrador.");
        Notes = notes?.Trim();
        SetUpdated(updatedBy);
    }

    public void AddLine(Guid documentId, decimal amount, string updatedBy)
    {
        if (Status != RemittanceStatus.Draft)
            throw new InvalidOperationException("Solo se pueden añadir líneas a remesas en estado Borrador.");
        if (documentId == Guid.Empty)
            throw new ArgumentException("El identificador del documento es obligatorio.", nameof(documentId));
        if (_lines.Any(l => l.DocumentId == documentId))
            throw new InvalidOperationException("El documento ya está incluido en esta remesa.");

        _lines.Add(RemittanceLine.Create(Id, documentId, amount));
        SetUpdated(updatedBy);
    }

    public void RemoveLine(Guid documentId, string updatedBy)
    {
        if (Status != RemittanceStatus.Draft)
            throw new InvalidOperationException("Solo se pueden eliminar líneas de remesas en estado Borrador.");

        var line = _lines.FirstOrDefault(l => l.DocumentId == documentId)
            ?? throw new InvalidOperationException("La línea no existe en esta remesa.");

        _lines.Remove(line);
        SetUpdated(updatedBy);
    }

    public void Send(string updatedBy)
    {
        if (Status != RemittanceStatus.Draft)
            throw new InvalidOperationException("Solo se puede enviar una remesa en estado Borrador.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("No se puede enviar una remesa sin líneas.");

        Status = RemittanceStatus.Sent;
        SetUpdated(updatedBy);
    }

    public void Confirm(string updatedBy)
    {
        if (Status != RemittanceStatus.Sent)
            throw new InvalidOperationException("Solo se puede confirmar una remesa en estado Enviada.");

        Status = RemittanceStatus.Confirmed;
        SetUpdated(updatedBy);
    }

    public void Fail(string reason, string updatedBy)
    {
        if (Status != RemittanceStatus.Sent)
            throw new InvalidOperationException("Solo se puede marcar como fallida una remesa en estado Enviada.");

        Status     = RemittanceStatus.Failed;
        FailReason = reason?.Trim();
        SetUpdated(updatedBy);
    }

    public void Delete(string deletedBy) => SoftDelete(deletedBy);
}
