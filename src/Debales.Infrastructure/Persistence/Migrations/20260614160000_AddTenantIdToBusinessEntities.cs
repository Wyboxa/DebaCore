using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToBusinessEntities : Migration
    {
        private static readonly string[] _excluded = [
            "__EFMigrationsHistory",
            "AuditEntries",
            "UserRoles", "RolePermissions", "Permissions",
            "SalesOrderLines", "SalesQuoteLines", "SalesDeliveryNoteLines",
            "SalesInvoiceLines", "SalesCreditNoteLines",
            "PurchaseOrderLines", "PurchaseDeliveryNoteLines",
            "PurchaseInvoiceLines", "PurchaseCreditNoteLines",
            "PaymentTermLines",
            "FiscalPeriods",
            "AccountingEntryLines", "AccountingTemplateLines",
            "StockBalances",
            "ItemPrices", "SupplierItemCodes", "CustomerItemCodes",
            "InventoryCountLines",
            "RemittanceLines",
            "LicenseModules",
            "AIActionApprovals", "AIExecutionLogs"
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var exclusions = string.Join(",", _excluded.Select(t => $"'{t}'"));
            migrationBuilder.Sql($@"
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'ALTER TABLE [' + TABLE_NAME + N'] ADD [TenantId] uniqueidentifier NULL;' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
  AND TABLE_NAME NOT IN ({exclusions})
  AND NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE Name = N'TenantId'
    AND OBJECT_ID = OBJECT_ID(TABLE_NAME)
  );

IF LEN(@sql) > 0
    EXEC sp_executesql @sql;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var exclusions = string.Join(",", _excluded.Select(t => $"'{t}'"));
            migrationBuilder.Sql($@"
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'ALTER TABLE [' + TABLE_NAME + N'] DROP COLUMN [TenantId];' + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
  AND TABLE_NAME NOT IN ({exclusions})
  AND EXISTS (
    SELECT 1 FROM sys.columns
    WHERE Name = N'TenantId'
    AND OBJECT_ID = OBJECT_ID(TABLE_NAME)
  );

IF LEN(@sql) > 0
    EXEC sp_executesql @sql;
");
        }
    }
}
