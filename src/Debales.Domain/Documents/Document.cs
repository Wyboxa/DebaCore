using Debales.Domain.Common;

namespace Debales.Domain.Documents;

public sealed class Document : AuditableEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid DocumentTypeId { get; private set; }
    public Guid? CustomerId { get; private set; }
    public Guid? SupplierId { get; private set; }
    public string? FileName { get; private set; }
    public long? FileSizeBytes { get; private set; }
    public string? MimeType { get; private set; }
    public string? Notes { get; private set; }
    public DateTime DocumentDate { get; private set; }
    public bool IsActive { get; private set; }

    private Document() { }

    public static Document Create(
        string title,
        string? description,
        Guid documentTypeId,
        Guid? customerId,
        Guid? supplierId,
        string? fileName,
        long? fileSizeBytes,
        string? mimeType,
        string? notes,
        DateTime documentDate,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío.", nameof(title));

        return new Document
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            DocumentTypeId = documentTypeId,
            CustomerId = customerId,
            SupplierId = supplierId,
            FileName = fileName?.Trim(),
            FileSizeBytes = fileSizeBytes,
            MimeType = mimeType?.Trim(),
            Notes = notes?.Trim(),
            DocumentDate = documentDate.Date,
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string title,
        string? description,
        Guid documentTypeId,
        Guid? customerId,
        Guid? supplierId,
        string? fileName,
        long? fileSizeBytes,
        string? mimeType,
        string? notes,
        DateTime documentDate,
        string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío.", nameof(title));

        Title = title.Trim();
        Description = description?.Trim();
        DocumentTypeId = documentTypeId;
        CustomerId = customerId;
        SupplierId = supplierId;
        FileName = fileName?.Trim();
        FileSizeBytes = fileSizeBytes;
        MimeType = mimeType?.Trim();
        Notes = notes?.Trim();
        DocumentDate = documentDate.Date;
        SetUpdated(updatedBy);
    }

    public void AttachFile(string fileName, long fileSizeBytes, string mimeType, string updatedBy)
    {
        FileName = fileName;
        FileSizeBytes = fileSizeBytes;
        MimeType = mimeType;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
