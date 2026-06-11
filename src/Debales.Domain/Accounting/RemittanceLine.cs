using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class RemittanceLine : Entity
{
    private RemittanceLine() { }

    public Guid RemittanceId { get; private set; }
    public Guid DocumentId { get; private set; }  // ReceivableId o PayableId según Type
    public decimal Amount { get; private set; }

    public Remittance? Remittance { get; private set; }

    internal static RemittanceLine Create(Guid remittanceId, Guid documentId, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("El importe de la línea debe ser mayor que cero.", nameof(amount));
        return new RemittanceLine { RemittanceId = remittanceId, DocumentId = documentId, Amount = amount };
    }
}
