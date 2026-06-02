namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal static class AccountingSeeds
{
    internal static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Journals
    internal static readonly Guid Journal_VTA = new("11111111-0000-0000-0000-000000000001");
    internal static readonly Guid Journal_CPR = new("11111111-0000-0000-0000-000000000002");
    internal static readonly Guid Journal_BCO = new("11111111-0000-0000-0000-000000000003");
    internal static readonly Guid Journal_CAJ = new("11111111-0000-0000-0000-000000000004");

    // Accounts
    internal static readonly Guid Account_300 = new("33333333-0000-0000-0000-000000000001");
    internal static readonly Guid Account_400 = new("33333333-0000-0000-0000-000000000002");
    internal static readonly Guid Account_430 = new("33333333-0000-0000-0000-000000000003");
    internal static readonly Guid Account_472 = new("33333333-0000-0000-0000-000000000004");
    internal static readonly Guid Account_475 = new("33333333-0000-0000-0000-000000000005");
    internal static readonly Guid Account_477 = new("33333333-0000-0000-0000-000000000006");
    internal static readonly Guid Account_570 = new("33333333-0000-0000-0000-000000000007");
    internal static readonly Guid Account_572 = new("33333333-0000-0000-0000-000000000008");
    internal static readonly Guid Account_600 = new("33333333-0000-0000-0000-000000000009");
    internal static readonly Guid Account_621 = new("33333333-0000-0000-0000-000000000010");
    internal static readonly Guid Account_628 = new("33333333-0000-0000-0000-000000000011");
    internal static readonly Guid Account_640 = new("33333333-0000-0000-0000-000000000012");
    internal static readonly Guid Account_700 = new("33333333-0000-0000-0000-000000000013");
    internal static readonly Guid Account_705 = new("33333333-0000-0000-0000-000000000014");

    // Templates
    internal static readonly Guid Template_SalesInvoicePosted = new("22222222-0000-0000-0000-000000000001");
    internal static readonly Guid Template_PurchaseInvoicePosted = new("22222222-0000-0000-0000-000000000002");

    // Template lines
    internal static readonly Guid TemplateLine_Sales_D_Customer = new("44444444-0000-0000-0000-000000000001");
    internal static readonly Guid TemplateLine_Sales_H_700 = new("44444444-0000-0000-0000-000000000002");
    internal static readonly Guid TemplateLine_Sales_H_477 = new("44444444-0000-0000-0000-000000000003");
    internal static readonly Guid TemplateLine_Purchase_D_600 = new("44444444-0000-0000-0000-000000000004");
    internal static readonly Guid TemplateLine_Purchase_D_472 = new("44444444-0000-0000-0000-000000000005");
    internal static readonly Guid TemplateLine_Purchase_H_Supplier = new("44444444-0000-0000-0000-000000000006");
}
