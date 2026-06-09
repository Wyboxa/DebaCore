using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPriceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountCode",
                table: "CrmCustomers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PriceListId",
                table: "CrmCustomers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomers_PriceListId",
                table: "CrmCustomers",
                column: "PriceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmCustomers_PriceLists_PriceListId",
                table: "CrmCustomers",
                column: "PriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmCustomers_PriceLists_PriceListId",
                table: "CrmCustomers");

            migrationBuilder.DropIndex(
                name: "IX_CrmCustomers_PriceListId",
                table: "CrmCustomers");

            migrationBuilder.DropColumn(
                name: "PriceListId",
                table: "CrmCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "AccountCode",
                table: "CrmCustomers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
