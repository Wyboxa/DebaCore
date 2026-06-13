using System.Text;

namespace Debales.Application.Common;

public static class CsvParser
{
    public static IReadOnlyList<string[]> Parse(string content, bool skipHeader = true)
    {
        var result = new List<string[]>();
        using var reader = new StringReader(content);
        bool first = true;
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (first && skipHeader) { first = false; continue; }
            first = false;
            result.Add(ParseLine(line));
        }
        return result;
    }

    public static string[] ParseHeader(string content)
    {
        using var reader = new StringReader(content);
        var line = reader.ReadLine();
        return line is null ? [] : ParseLine(line);
    }

    private static string[] ParseLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else if (c == '"')
                {
                    inQuotes = false;
                }
                else
                {
                    current.Append(c);
                }
            }
            else if (c == '"')
            {
                inQuotes = true;
            }
            else if (c == ',')
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        fields.Add(current.ToString().Trim());
        return [.. fields];
    }

    public static string? Field(string[] row, int index) =>
        index < row.Length && !string.IsNullOrWhiteSpace(row[index]) ? row[index] : null;

    public static decimal ParseDecimal(string[] row, int index, decimal fallback = 0) =>
        index < row.Length && decimal.TryParse(
            row[index].Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out var v) ? v : fallback;

    public static bool ParseBool(string[] row, int index) =>
        index < row.Length &&
        row[index].Trim().ToLowerInvariant() is "true" or "si" or "sí" or "1" or "yes";
}
