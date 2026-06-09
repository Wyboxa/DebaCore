using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTermsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountCode",
                table: "Suppliers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentTermId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentTermId",
                table: "CrmCustomers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTermLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DueDays = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTermLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTermLines_PaymentTerms_PaymentTermId",
                        column: x => x.PaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_PaymentTermId",
                table: "Suppliers",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomers_PaymentTermId",
                table: "CrmCustomers",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTermLines_PaymentTermId",
                table: "PaymentTermLines",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerms_Name",
                table: "PaymentTerms",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmCustomers_PaymentTerms_PaymentTermId",
                table: "CrmCustomers",
                column: "PaymentTermId",
                principalTable: "PaymentTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_PaymentTerms_PaymentTermId",
                table: "Suppliers",
                column: "PaymentTermId",
                principalTable: "PaymentTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmCustomers_PaymentTerms_PaymentTermId",
                table: "CrmCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_PaymentTerms_PaymentTermId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "PaymentTermLines");

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_PaymentTermId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_CrmCustomers_PaymentTermId",
                table: "CrmCustomers");

            migrationBuilder.DropColumn(
                name: "PaymentTermId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PaymentTermId",
                table: "CrmCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "AccountCode",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
