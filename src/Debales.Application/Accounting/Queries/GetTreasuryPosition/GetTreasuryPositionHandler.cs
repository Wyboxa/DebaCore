using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetTreasuryPosition;

public sealed class GetTreasuryPositionHandler(
    IBankAccountRepository banks,
    ICashAccountRepository cashAccounts)
{
    public async Task<TreasuryPositionDto> Handle(GetTreasuryPositionQuery query, CancellationToken ct = default)
    {
        var bankList = await banks.GetAllActiveAsync(ct);
        var cashList = await cashAccounts.GetAllActiveAsync(ct);

        var bankDtos = bankList
            .Select(b => new TreasuryAccountDto(b.Id, b.Name, "Banco", b.CurrencyCode, 0m))
            .ToList();

        var cashDtos = cashList
            .Select(c => new TreasuryAccountDto(c.Id, c.Name, "Caja", c.CurrencyCode, c.CurrentBalance))
            .ToList();

        var totalBank = bankDtos.Sum(b => b.Balance);
        var totalCash = cashDtos.Sum(c => c.Balance);

        return new TreasuryPositionDto(
            DateOnly.FromDateTime(DateTime.Today),
            totalBank, totalCash, totalBank + totalCash,
            bankDtos, cashDtos);
    }
}
