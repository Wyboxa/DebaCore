using ClosedXML.Excel;

namespace Debales.Application.Common.Export;

public static class ExcelExporter
{
    public static byte[] Generate(string sheetName, IReadOnlyList<string> headers, IEnumerable<IReadOnlyList<object?>> rows)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add(sheetName);

        for (int c = 0; c < headers.Count; c++)
        {
            var cell = ws.Cell(1, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#6B9CA9");
            cell.Style.Font.FontColor = XLColor.White;
        }

        int row = 2;
        foreach (var dataRow in rows)
        {
            for (int c = 0; c < dataRow.Count; c++)
            {
                var v = dataRow[c];
                if (v is null) continue;
                var cell = ws.Cell(row, c + 1);
                if (v is bool b) cell.Value = b;
                else if (v is decimal d) cell.Value = d;
                else if (v is int i) cell.Value = i;
                else if (v is DateTime dt) { cell.Value = dt; cell.Style.DateFormat.Format = "dd/MM/yyyy"; }
                else cell.Value = v.ToString();
            }
            row++;
        }

        ws.RangeUsed()?.SetAutoFilter();
        ws.Columns().AdjustToContents(8, 60);

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
