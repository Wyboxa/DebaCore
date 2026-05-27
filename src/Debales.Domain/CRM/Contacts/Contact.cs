using Debales.Domain.Common;

namespace Debales.Domain.CRM.Contacts;

public sealed class Contact : AuditableEntity
{
    public Guid CustomerId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? JobTitle { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    private Contact() { }

    public static Contact Create(
        Guid customerId,
        string firstName,
        string lastName,
        string? jobTitle,
        string? email,
        string? phone,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido no puede estar vacío.", nameof(lastName));

        return new Contact
        {
            CustomerId = customerId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            JobTitle = jobTitle?.Trim(),
            Email = email?.Trim().ToLowerInvariant(),
            Phone = phone?.Trim(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
