using Debales.Domain.Common;

namespace Debales.Domain.Suppliers;

public sealed class SupplierAddress : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Country { get; }

    private SupplierAddress(string street, string city, string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static SupplierAddress Create(string street, string city, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("La ciudad no puede estar vacía.", nameof(city));

        return new SupplierAddress(
            street.Trim(),
            city.Trim(),
            postalCode.Trim(),
            country.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
        yield return Country;
    }
}
