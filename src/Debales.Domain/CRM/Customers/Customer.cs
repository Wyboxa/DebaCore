using Debales.Domain.Common;
using Debales.Domain.CRM.Activities;
using Debales.Domain.CRM.Contacts;
using Debales.Domain.CRM.Notes;
using Debales.Domain.CRM.Opportunities;

namespace Debales.Domain.CRM.Customers;

public sealed class Customer : AuditableEntity
{
    private readonly List<Contact> _contacts = [];
    private readonly List<Activity> _activities = [];
    private readonly List<Note> _notes = [];
    private readonly List<Opportunity> _opportunities = [];

    public string Name { get; private set; } = string.Empty;
    public string? Sector { get; private set; }
    public string? TaxId { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Website { get; private set; }
    public bool IsActive { get; private set; }
    public Address? Address { get; private set; }
    public string? AccountCode { get; private set; }
    public Guid? PriceListId { get; private set; }
    public Guid? PaymentTermId { get; private set; }

    public IReadOnlyList<Contact> Contacts => _contacts.AsReadOnly();
    public IReadOnlyList<Activity> Activities => _activities.AsReadOnly();
    public IReadOnlyList<Note> Notes => _notes.AsReadOnly();
    public IReadOnlyList<Opportunity> Opportunities => _opportunities.AsReadOnly();

    private Customer() { }

    public static Customer Create(string name, string? sector, string? taxId, string? phone, string? email, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del cliente no puede estar vacío.", nameof(name));

        return new Customer
        {
            Name = name.Trim(),
            Sector = sector?.Trim(),
            TaxId = taxId?.Trim(),
            Phone = phone?.Trim(),
            Email = email?.Trim().ToLowerInvariant(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string? sector, string? taxId, string? phone, string? email, string? website, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del cliente no puede estar vacío.", nameof(name));

        Name = name.Trim();
        Sector = sector?.Trim();
        TaxId = taxId?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim().ToLowerInvariant();
        Website = website?.Trim();
        SetUpdated(updatedBy);
    }

    public void SetAddress(Address? address, string updatedBy)
    {
        Address = address;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    public void SetAccountCode(string? accountCode, string updatedBy)
    {
        AccountCode = accountCode?.Trim().ToUpper();
        SetUpdated(updatedBy);
    }

    public void SetPriceList(Guid? priceListId, string updatedBy)
    {
        PriceListId = priceListId;
        SetUpdated(updatedBy);
    }

    public void SetPaymentTerm(Guid? paymentTermId, string updatedBy)
    {
        PaymentTermId = paymentTermId;
        SetUpdated(updatedBy);
    }
}
