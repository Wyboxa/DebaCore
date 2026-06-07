using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAccountingTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountingTemplates",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EventType", "IsActive", "IsDeleted", "JournalCode", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000003"), "CUST_PAY_CONFIRMED", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, "CustomerPaymentConfirmed", true, false, "BCO", "Cobro de cliente", null, null },
                    { new Guid("22222222-0000-0000-0000-000000000004"), "SUPP_PAY_CONFIRMED", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", null, null, "SupplierPaymentConfirmed", true, false, "BCO", "Pago a proveedor", null, null }
                });

            migrationBuilder.InsertData(
                table: "AccountingTemplateLines",
                columns: new[] { "Id", "AccountCode", "AccountingTemplateId", "AmountType", "CreatedAt", "CreatedBy", "Description", "Side", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000007"), "572", new Guid("22222222-0000-0000-0000-000000000003"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bancos — cobro recibido", 0, 1, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000008"), "{CUSTOMER}", new Guid("22222222-0000-0000-0000-000000000003"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cliente — cancelación de deuda", 1, 2, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000009"), "{SUPPLIER}", new Guid("22222222-0000-0000-0000-000000000004"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Proveedor — cancelación de deuda", 0, 1, null, null },
                    { new Guid("44444444-0000-0000-0000-000000000010"), "572", new Guid("22222222-0000-0000-0000-000000000004"), 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bancos — pago realizado", 1, 2, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountingTemplateLines",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "AccountingTemplateLines",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "AccountingTemplateLines",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "AccountingTemplateLines",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "AccountingTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AccountingTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000004"));
        }
    }
}
