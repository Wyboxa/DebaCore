using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "CrmCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountingJournals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingJournals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountingTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JournalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsPostable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountingTemplateLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountingTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AmountType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingTemplateLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingTemplateLines_AccountingTemplates_AccountingTemplateId",
                        column: x => x.AccountingTemplateId,
                        principalTable: "AccountingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FiscalYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalPeriods_FiscalYears_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalTable: "FiscalYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JournalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FiscalPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingEntries_AccountingJournals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "AccountingJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingEntries_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountingEntryLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountingEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ThirdPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThirdPartyType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingEntryLines_AccountingEntries_AccountingEntryId",
                        column: x => x.AccountingEntryId,
                        principalTable: "AccountingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountingEntryLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AccountingJournals",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsActive", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), "VTA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, "Diario de Ventas", null, null },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "CPR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, "Diario de Compras", null, null },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "BCO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, "Diario de Banco", null, null },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "CAJ", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, "Diario de Caja", null, null }
                });

            migrationBuilder.InsertData(
                table: "AccountingTemplates",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EventType", "IsActive", "IsDeleted", "JournalCode", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), "SALES_INV_POSTED", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, "SalesInvoicePosted", true, false, "VTA", "Factura de venta contabilizada", null, null },
                    { new Guid("22222222-0000-0000-0000-000000000002"), "PURCH_INV_POSTED", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, "PurchaseInvoicePosted", true, false, "CPR", "Factura de compra contabilizada", null, null }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsActive", "IsDeleted", "IsPostable", "Name", "ParentCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), "300", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Mercaderías", null, 0, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000002"), "400", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, false, "Proveedores", null, 1, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000003"), "430", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, false, "Clientes", null, 0, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000004"), "472", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "HP IVA soportado", null, 0, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000005"), "475", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "HP acreedora por conceptos fiscales", null, 1, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000006"), "477", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "HP IVA repercutido", null, 1, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000007"), "570", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Caja", null, 0, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000008"), "572", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Bancos c/c", null, 0, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000009"), "600", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Compras de mercaderías", null, 4, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000010"), "621", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Arrendamientos y cánones", null, 4, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000011"), "628", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Suministros", null, 4, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000012"), "640", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Sueldos y salarios", null, 4, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000013"), "700", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Ventas de mercaderías", null, 3, null, null },
                    { new Guid("33333333-0000-0000-0000-000000000014"), "705", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, true, false, true, "Prestaciones de servicios", null, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "AccountingTemplateLines",
                columns: new[] { "Id", "AccountCode", "AccountingTemplateId", "AmountType", "CreatedAt", "CreatedBy", "Description", "Side", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000001"), "{CUSTOMER}", new Guid("22222222-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cliente — total factura", 0, 1, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000002"), "700", new Guid("22222222-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ventas de mercaderías", 1, 2, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000003"), "477", new Guid("22222222-0000-0000-0000-000000000001"), 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "IVA repercutido", 1, 3, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000004"), "600", new Guid("22222222-0000-0000-0000-000000000002"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Compras de mercaderías", 0, 1, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000005"), "472", new Guid("22222222-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "IVA soportado", 0, 2, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000006"), "{SUPPLIER}", new Guid("22222222-0000-0000-0000-000000000002"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Proveedor — total factura", 1, 3, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_FiscalPeriodId",
                table: "AccountingEntries",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_JournalId",
                table: "AccountingEntries",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_Number",
                table: "AccountingEntries",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntryLines_AccountId",
                table: "AccountingEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntryLines_AccountingEntryId",
                table: "AccountingEntryLines",
                column: "AccountingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingJournals_Code",
                table: "AccountingJournals",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTemplateLines_AccountingTemplateId",
                table: "AccountingTemplateLines",
                column: "AccountingTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTemplates_Code",
                table: "AccountingTemplates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTemplates_EventType",
                table: "AccountingTemplates",
                column: "EventType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Code",
                table: "Accounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_FiscalYearId",
                table: "FiscalPeriods",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_Name",
                table: "FiscalYears",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntryLines");

            migrationBuilder.DropTable(
                name: "AccountingTemplateLines");

            migrationBuilder.DropTable(
                name: "AccountingEntries");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountingTemplates");

            migrationBuilder.DropTable(
                name: "AccountingJournals");

            migrationBuilder.DropTable(
                name: "FiscalPeriods");

            migrationBuilder.DropTable(
                name: "FiscalYears");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "CrmCustomers");
        }
    }
}
