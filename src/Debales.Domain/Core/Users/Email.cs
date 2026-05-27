using Debales.Domain.Common;

namespace Debales.Domain.Core.Users;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío.", nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!value.Contains('@') || !value.Contains('.'))
            throw new ArgumentException("El email no tiene un formato válido.", nameof(value));

        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
