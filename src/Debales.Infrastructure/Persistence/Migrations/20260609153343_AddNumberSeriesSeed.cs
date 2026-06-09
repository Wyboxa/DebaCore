using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberSeriesSeed : Migration
    {
        private static readonly DateTime _now = new(2026, 6, 9, 0, 0, 0, DateTimeKind.Utc);

        private static readonly (string Id, string Code, string Name, string Prefix)[] _series =
        [
            ("11111111-0001-0000-0000-000000000000", "PRE", "Presupuestos de venta",   "PRE-"),
            ("11111111-0002-0000-0000-000000000000", "PV",  "Pedidos de venta",        "PV-"),
            ("11111111-0003-0000-0000-000000000000", "ALV", "Albaranes de venta",      "ALV-"),
            ("11111111-0004-0000-0000-000000000000", "FV",  "Facturas de venta",       "FV-"),
            ("11111111-0005-0000-0000-000000000000", "RV",  "Rectificativas de venta", "RV-"),
            ("11111111-0006-0000-0000-000000000000", "PC",  "Pedidos de compra",       "PC-"),
            ("11111111-0007-0000-0000-000000000000", "ALC", "Albaranes de compra",     "ALC-"),
            ("11111111-0008-0000-0000-000000000000", "FC",  "Facturas de compra",      "FC-"),
            ("11111111-0009-0000-0000-000000000000", "RC",  "Rectificativas de compra","RC-"),
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var s in _series)
            {
                migrationBuilder.InsertData(
                    table: "NumberSeries",
                    columns: ["Id", "Code", "Name", "Prefix", "Padding", "NextNumber", "IsActive", "CreatedAt", "CreatedBy"],
                    values: [Guid.Parse(s.Id), s.Code, s.Name, s.Prefix, 5, 1, true, _now, "system"]);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var s in _series)
                migrationBuilder.DeleteData(table: "NumberSeries", keyColumn: "Id", keyValue: Guid.Parse(s.Id));
        }
    }
}
