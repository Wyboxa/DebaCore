using Debales.Application.Common;

namespace Debales.Application.Tests.Common;

public sealed class CsvParserTests
{
    [Fact]
    public void Parse_SimpleRow_ReturnsFields()
    {
        var csv = "Nombre,Sector,Email\nEmpresa A,Retail,a@a.es";
        var rows = CsvParser.Parse(csv);
        Assert.Single(rows);
        Assert.Equal("Empresa A", rows[0][0]);
        Assert.Equal("Retail", rows[0][1]);
        Assert.Equal("a@a.es", rows[0][2]);
    }

    [Fact]
    public void Parse_QuotedField_HandlesCommaInside()
    {
        var csv = "Name,City\n\"García, S.L.\",Madrid";
        var rows = CsvParser.Parse(csv);
        Assert.Equal("García, S.L.", rows[0][0]);
        Assert.Equal("Madrid", rows[0][1]);
    }

    [Fact]
    public void Parse_EscapedQuote_HandlesDoubleQuote()
    {
        var csv = "Name\n\"He said \"\"hello\"\"\"";
        var rows = CsvParser.Parse(csv);
        Assert.Equal("He said \"hello\"", rows[0][0]);
    }

    [Fact]
    public void Parse_SkipsEmptyLines()
    {
        var csv = "H1,H2\nA,B\n\nC,D";
        var rows = CsvParser.Parse(csv);
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void ParseDecimal_CommaOrDotSeparator_ParsesCorrectly()
    {
        string[] row = ["", "", "19,99"];
        Assert.Equal(19.99m, CsvParser.ParseDecimal(row, 2));
    }

    [Fact]
    public void ParseBool_VariousValues_DetectsTrue()
    {
        Assert.True(CsvParser.ParseBool(["true"], 0));
        Assert.True(CsvParser.ParseBool(["si"], 0));
        Assert.True(CsvParser.ParseBool(["1"], 0));
        Assert.False(CsvParser.ParseBool(["false"], 0));
        Assert.False(CsvParser.ParseBool(["no"], 0));
    }

    [Fact]
    public void Field_EmptyValue_ReturnsNull()
    {
        string[] row = ["Nombre", "", "  "];
        Assert.NotNull(CsvParser.Field(row, 0));
        Assert.Null(CsvParser.Field(row, 1));
        Assert.Null(CsvParser.Field(row, 2));
    }
}
