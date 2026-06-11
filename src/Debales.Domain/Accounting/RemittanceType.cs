namespace Debales.Domain.Accounting;

public enum RemittanceType
{
    Collection = 1, // agrupa cobros (Receivable)
    Payment    = 2  // agrupa pagos (Payable)
}
