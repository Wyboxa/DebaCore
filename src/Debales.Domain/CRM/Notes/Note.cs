using Debales.Domain.Common;

namespace Debales.Domain.CRM.Notes;

public sealed class Note : Entity
{
    public Guid CustomerId { get; private set; }
    public string Content { get; private set; } = string.Empty;

    private Note() { }

    public static Note Create(Guid customerId, string content, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("El contenido de la nota no puede estar vacío.", nameof(content));

        return new Note
        {
            CustomerId = customerId,
            Content = content.Trim(),
            CreatedBy = createdBy
        };
    }
}
