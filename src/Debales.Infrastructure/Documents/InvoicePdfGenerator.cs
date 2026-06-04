using Debales.Application.Documents;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Sales.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Debales.Infrastructure.Documents;

public sealed class InvoicePdfGenerator : IInvoicePdfGenerator
{
    static InvoicePdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateSalesInvoice(SalesInvoiceDetailDto invoice)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, "FACTURA DE VENTA", invoice.Number,
                    invoice.Date, invoice.DueDate, invoice.Status.ToString()));

                page.Content().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Cliente").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.CustomerName).SemiBold();
                        });
                        row.ConstantItem(120).Column(c =>
                        {
                            c.Item().Text("Fecha").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.Date.ToString("dd/MM/yyyy"));
                            c.Item().PaddingTop(4).Text("Vencimiento").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.DueDate.ToString("dd/MM/yyyy"));
                        });
                    });

                    col.Item().PaddingTop(16).Element(c => ComposeInvoiceLines(c,
                        invoice.Lines.Select(l => (l.SortOrder, l.ItemCode, l.Description ?? l.ItemName,
                            l.Quantity, l.UnitPrice, l.TaxRate, l.LineSubtotal, l.LineTaxAmount, l.LineTotal)).ToList()));

                    col.Item().PaddingTop(8).Element(c => ComposeTotals(c, invoice.Subtotal, invoice.TaxAmount, invoice.Total));

                    if (!string.IsNullOrEmpty(invoice.Notes))
                        col.Item().PaddingTop(12).Text(invoice.Notes).Italic().FontSize(9).FontColor(Colors.Grey.Darken1);
                });

                page.Footer().AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Debales ERP · ").FontColor(Colors.Grey.Darken1);
                        x.Span(invoice.Number).SemiBold();
                        x.Span(" · Página ").FontColor(Colors.Grey.Darken1);
                        x.CurrentPageNumber();
                        x.Span(" de ").FontColor(Colors.Grey.Darken1);
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();
    }

    public byte[] GeneratePurchaseInvoice(PurchaseInvoiceDetailDto invoice)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, "FACTURA DE COMPRA", invoice.Number,
                    invoice.Date, invoice.DueDate, invoice.Status.ToString()));

                page.Content().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Proveedor").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.SupplierName).SemiBold();
                            if (!string.IsNullOrEmpty(invoice.SupplierInvoiceNumber))
                            {
                                c.Item().PaddingTop(4).Text("Nº factura proveedor").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text(invoice.SupplierInvoiceNumber);
                            }
                        });
                        row.ConstantItem(120).Column(c =>
                        {
                            c.Item().Text("Fecha").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.Date.ToString("dd/MM/yyyy"));
                            c.Item().PaddingTop(4).Text("Vencimiento").SemiBold().FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(invoice.DueDate.ToString("dd/MM/yyyy"));
                        });
                    });

                    col.Item().PaddingTop(16).Element(c => ComposeInvoiceLines(c,
                        invoice.Lines.Select(l => (l.SortOrder, l.ItemCode, l.Description ?? l.ItemName,
                            l.Quantity, l.UnitPrice, l.TaxRate, l.LineSubtotal, l.LineTaxAmount, l.LineTotal)).ToList()));

                    col.Item().PaddingTop(8).Element(c => ComposeTotals(c, invoice.Subtotal, invoice.TaxAmount, invoice.Total));

                    if (!string.IsNullOrEmpty(invoice.Notes))
                        col.Item().PaddingTop(12).Text(invoice.Notes).Italic().FontSize(9).FontColor(Colors.Grey.Darken1);
                });

                page.Footer().AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Debales ERP · ").FontColor(Colors.Grey.Darken1);
                        x.Span(invoice.Number).SemiBold();
                        x.Span(" · Página ").FontColor(Colors.Grey.Darken1);
                        x.CurrentPageNumber();
                        x.Span(" de ").FontColor(Colors.Grey.Darken1);
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();
    }

    // ── helpers ─────────────────────────────────────────────────────────────

    private static void ComposeHeader(IContainer container, string docType, string number,
        DateOnly date, DateOnly dueDate, string status)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Debales").FontSize(22).SemiBold().FontColor(Color.FromHex("#6B9CA9"));
                    c.Item().Text("Plataforma CRM/ERP").FontSize(9).FontColor(Colors.Grey.Darken1);
                });
                row.ConstantItem(180).Column(c =>
                {
                    c.Item().Text(docType).FontSize(14).SemiBold().FontColor(Color.FromHex("#1e2a35"));
                    c.Item().Text(number).FontSize(12).SemiBold();
                    c.Item().PaddingTop(2).Text($"{date:dd/MM/yyyy}").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Background(Color.FromHex(status == "Posted" ? "#d1fae5" : "#f3f4f6"))
                            .Padding(3).Text(status).FontSize(8).FontColor(Color.FromHex("#374151"));
                });
            });
            col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Color.FromHex("#6B9CA9"));
            col.Item().Height(8);
        });
    }

    private static void ComposeInvoiceLines(IContainer container,
        List<(int Sort, string Code, string Desc, decimal Qty, decimal Price, decimal Tax, decimal Sub, decimal TaxAmt, decimal Total)> lines)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.ConstantColumn(24);   // #
                c.ConstantColumn(60);   // Código
                c.RelativeColumn(3);    // Descripción
                c.ConstantColumn(50);   // Cantidad
                c.ConstantColumn(65);   // P.Unit
                c.ConstantColumn(40);   // IVA%
                c.ConstantColumn(65);   // Subtotal
                c.ConstantColumn(65);   // Total
            });

            // Header row
            static IContainer HeaderCell(IContainer c) =>
                c.Background(Color.FromHex("#1e2a35")).Padding(4);

            table.Header(h =>
            {
                string[] leftLabels  = ["#", "Código", "Descripción"];
                string[] rightLabels = ["Cant.", "P.Unit", "IVA%", "Subtotal", "Total"];

                foreach (var label in leftLabels)
                    h.Cell().Element(HeaderCell).Text(label).FontSize(8).SemiBold().FontColor(Colors.White);

                foreach (var label in rightLabels)
                    h.Cell().Element(HeaderCell).AlignRight().Text(label).FontSize(8).SemiBold().FontColor(Colors.White);
            });

            // Data rows
            for (var i = 0; i < lines.Count; i++)
            {
                var l = lines[i];
                var bg = i % 2 == 0 ? Colors.White : Color.FromHex("#f9fafb");

                static IContainer DataCell(IContainer c, string bg) =>
                    c.Background(Color.FromHex(bg)).BorderBottom(0.5f).BorderColor(Color.FromHex("#e5e7eb")).Padding(4);

                table.Cell().Element(c => DataCell(c, bg)).Text(l.Sort.ToString()).FontSize(9).FontColor(Colors.Grey.Darken1);
                table.Cell().Element(c => DataCell(c, bg)).Text(l.Code).FontSize(9).FontFamily("Courier New");
                table.Cell().Element(c => DataCell(c, bg)).Text(l.Desc).FontSize(9);
                table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(l.Qty.ToString("N2")).FontSize(9);
                table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(l.Price.ToString("N4")).FontSize(9);
                table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text($"{l.Tax:N0}%").FontSize(9);
                table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(l.Sub.ToString("N2")).FontSize(9);
                table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(l.Total.ToString("N2")).FontSize(9).SemiBold();
            }
        });
    }

    private static void ComposeTotals(IContainer container, decimal subtotal, decimal tax, decimal total)
    {
        container.AlignRight().Width(220).Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().Text("Base imponible:").FontSize(9).FontColor(Colors.Grey.Darken1);
                r.ConstantItem(80).AlignRight().Text($"{subtotal:N2} €").FontSize(9);
            });
            col.Item().PaddingTop(2).Row(r =>
            {
                r.RelativeItem().Text("IVA:").FontSize(9).FontColor(Colors.Grey.Darken1);
                r.ConstantItem(80).AlignRight().Text($"{tax:N2} €").FontSize(9);
            });
            col.Item().PaddingTop(4).Background(Color.FromHex("#1e2a35")).Padding(6).Row(r =>
            {
                r.RelativeItem().Text("TOTAL:").FontSize(11).SemiBold().FontColor(Colors.White);
                r.ConstantItem(90).AlignRight().Text($"{total:N2} €").FontSize(11).SemiBold().FontColor(Colors.White);
            });
        });
    }
}
