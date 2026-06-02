using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string Code, string Name, AccountType Type,
    bool IsPostable, string? ParentCode, string CreatedBy);
