using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class SalesQuote : AuditableEntity
{
    private readonly List<SalesQuoteLine> _lines = [];

    private SalesQuote() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public DateOnly Date { get; private set; }
    public DateOnly ValidUntil { get; private set; }
    public SalesQuoteStatus Status { get; private set; } = SalesQuoteStatus.Draft;
    public string? Notes { get; private set; }
    public Guid? ConvertedToOrderId { get; private set; }

    public Customer? Customer { get; private set; }
    public IReadOnlyList<SalesQuoteLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static SalesQuote Create(
        string number, Guid customerId, DateOnly date,
        DateOnly validUntil, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));
        if (validUntil < date) throw new ArgumentException("ValidUntil must be on or after Date.", nameof(validUntil));

        return new SalesQuote
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            Date = date,
            ValidUntil = validUntil,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public SalesQuoteLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != SalesQuoteStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft quotes.");

        var line = SalesQuoteLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Send(string updatedBy)
    {
        if (Status != SalesQuoteStatus.Draft)
            throw new InvalidOperationException("Only draft quotes can be sent.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot send a quote with no lines.");

        Status = SalesQuoteStatus.Sent;
        SetUpdated(updatedBy);
    }

    public void Accept(string updatedBy)
    {
        if (Status != SalesQuoteStatus.Sent)
            throw new InvalidOperationException("Only sent quotes can be accepted.");

        Status = SalesQuoteStatus.Accepted;
        SetUpdated(updatedBy);
    }

    public void Reject(string updatedBy)
    {
        if (Status is not (SalesQuoteStatus.Sent or SalesQuoteStatus.Draft))
            throw new InvalidOperationException("Only draft or sent quotes can be rejected.");

        Status = SalesQuoteStatus.Rejected;
        SetUpdated(updatedBy);
    }

    public void MarkExpired(string updatedBy)
    {
        if (Status is SalesQuoteStatus.Accepted or SalesQuoteStatus.Rejected or SalesQuoteStatus.Expired)
            throw new InvalidOperationException($"Cannot expire a quote in status {Status}.");

        Status = SalesQuoteStatus.Expired;
        SetUpdated(updatedBy);
    }

    public void MarkConvertedToOrder(Guid orderId, string updatedBy)
    {
        if (Status != SalesQuoteStatus.Accepted)
            throw new InvalidOperationException("Only accepted quotes can be converted to order.");
        if (ConvertedToOrderId.HasValue)
            throw new InvalidOperationException("This quote has already been converted to an order.");

        ConvertedToOrderId = orderId;
        SetUpdated(updatedBy);
    }
}
