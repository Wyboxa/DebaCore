using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateAccount;

public sealed class CreateAccountHandler
{
    private readonly IAccountRepository _accounts;
    private readonly IUnitOfWork _uow;

    public CreateAccountHandler(IAccountRepository accounts, IUnitOfWork uow)
    {
        _accounts = accounts;
        _uow = uow;
    }

    public async Task<AccountSummaryDto> Handle(CreateAccountCommand command, CancellationToken ct = default)
    {
        if (await _accounts.ExistsByCodeAsync(command.Code, ct))
            throw new InvalidOperationException($"Ya existe una cuenta con el código '{command.Code}'.");

        var account = Account.Create(command.Code, command.Name, command.Type, command.IsPostable, command.ParentCode, command.CreatedBy);
        await _accounts.AddAsync(account, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(account);
    }

    internal static AccountSummaryDto ToDto(Account a) => new(
        a.Id, a.Code, a.Name, a.Type, TypeLabel(a.Type),
        a.IsPostable, a.IsActive, a.ParentCode);

    internal static AccountDetailDto ToDetailDto(Account a) => new(
        a.Id, a.Code, a.Name, a.Type, TypeLabel(a.Type),
        a.IsPostable, a.IsActive, a.ParentCode,
        a.CreatedAt, a.CreatedBy, a.UpdatedAt, a.UpdatedBy);

    internal static string TypeLabel(AccountType t) => t switch
    {
        AccountType.Asset => "Activo",
        AccountType.Liability => "Pasivo",
        AccountType.Equity => "Patrimonio",
        AccountType.Revenue => "Ingreso",
        AccountType.Expense => "Gasto",
        _ => t.ToString()
    };
}
