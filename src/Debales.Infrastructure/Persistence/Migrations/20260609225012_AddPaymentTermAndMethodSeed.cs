using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTermAndMethodSeed : Migration
    {
        private static readonly DateTime _now = new(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc);

        private static readonly (string Id, string Name, string? Description)[] _terms =
        [
            ("22222222-0001-0000-0000-000000000000", "Contado",       "Pago en el momento de la transacción"),
            ("22222222-0002-0000-0000-000000000000", "30 días",       "Pago a 30 días desde la fecha de factura"),
            ("22222222-0003-0000-0000-000000000000", "45 días",       "Pago a 45 días desde la fecha de factura"),
            ("22222222-0004-0000-0000-000000000000", "60 días",       "Pago a 60 días desde la fecha de factura"),
            ("22222222-0005-0000-0000-000000000000", "90 días",       "Pago a 90 días desde la fecha de factura"),
            ("22222222-0006-0000-0000-000000000000", "30/60 días",    "Pago en dos plazos iguales a 30 y 60 días"),
            ("22222222-0007-0000-0000-000000000000", "30/60/90 días", "Pago en tres plazos iguales a 30, 60 y 90 días"),
        ];

        private static readonly (string TermId, string LineId, int DueDays, decimal Percentage, int Sort)[] _lines =
        [
            ("22222222-0001-0000-0000-000000000000", "22222222-0001-0001-0000-000000000000",  0,   100m,   1),
            ("22222222-0002-0000-0000-000000000000", "22222222-0002-0001-0000-000000000000",  30,  100m,   1),
            ("22222222-0003-0000-0000-000000000000", "22222222-0003-0001-0000-000000000000",  45,  100m,   1),
            ("22222222-0004-0000-0000-000000000000", "22222222-0004-0001-0000-000000000000",  60,  100m,   1),
            ("22222222-0005-0000-0000-000000000000", "22222222-0005-0001-0000-000000000000",  90,  100m,   1),
            ("22222222-0006-0000-0000-000000000000", "22222222-0006-0001-0000-000000000000",  30,  50m,    1),
            ("22222222-0006-0000-0000-000000000000", "22222222-0006-0002-0000-000000000000",  60,  50m,    2),
            ("22222222-0007-0000-0000-000000000000", "22222222-0007-0001-0000-000000000000",  30,  33.34m, 1),
            ("22222222-0007-0000-0000-000000000000", "22222222-0007-0002-0000-000000000000",  60,  33.33m, 2),
            ("22222222-0007-0000-0000-000000000000", "22222222-0007-0003-0000-000000000000",  90,  33.33m, 3),
        ];

        private static readonly (string Id, string Name, string Code, string Description)[] _methods =
        [
            ("33333333-0001-0000-0000-000000000000", "Contado",                   "CASH",  "Pago en efectivo en el momento"),
            ("33333333-0002-0000-0000-000000000000", "Transferencia bancaria",    "WIRE",  "Transferencia SEPA o internacional"),
            ("33333333-0003-0000-0000-000000000000", "Domiciliación SEPA",        "SEPA",  "Cargo directo en cuenta bancaria"),
            ("33333333-0004-0000-0000-000000000000", "Tarjeta de crédito/débito", "CARD",  "Pago con tarjeta bancaria"),
            ("33333333-0005-0000-0000-000000000000", "Cheque",                    "CHECK", "Pago mediante cheque bancario"),
            ("33333333-0006-0000-0000-000000000000", "Pagaré",                    "NOTE",  "Efecto comercial a vencimiento"),
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var t in _terms)
            {
                migrationBuilder.InsertData(
                    table: "PaymentTerms",
                    columns: ["Id", "Name", "Description", "IsActive", "IsDeleted", "CreatedAt", "CreatedBy"],
                    values: [Guid.Parse(t.Id), t.Name, (object?)t.Description ?? DBNull.Value, true, false, _now, "system"]);
            }

            foreach (var l in _lines)
            {
                migrationBuilder.InsertData(
                    table: "PaymentTermLines",
                    columns: ["Id", "PaymentTermId", "DueDays", "Percentage", "SortOrder", "CreatedAt"],
                    values: [Guid.Parse(l.LineId), Guid.Parse(l.TermId), l.DueDays, l.Percentage, l.Sort, _now]);
            }

            foreach (var m in _methods)
            {
                migrationBuilder.InsertData(
                    table: "PaymentMethods",
                    columns: ["Id", "Name", "Code", "Description", "IsActive", "IsDeleted", "CreatedAt", "CreatedBy"],
                    values: [Guid.Parse(m.Id), m.Name, m.Code, m.Description, true, false, _now, "system"]);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var l in _lines)
                migrationBuilder.DeleteData(table: "PaymentTermLines", keyColumn: "Id", keyValue: Guid.Parse(l.LineId));
            foreach (var t in _terms)
                migrationBuilder.DeleteData(table: "PaymentTerms", keyColumn: "Id", keyValue: Guid.Parse(t.Id));
            foreach (var m in _methods)
                migrationBuilder.DeleteData(table: "PaymentMethods", keyColumn: "Id", keyValue: Guid.Parse(m.Id));
        }
    }
}
