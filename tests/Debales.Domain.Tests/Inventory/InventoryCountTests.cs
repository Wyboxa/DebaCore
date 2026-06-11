using Debales.Domain.Inventory;

namespace Debales.Domain.Tests.Inventory;

public sealed class InventoryCountTests
{
    private static InventoryCount CreateOpen()
    {
        return InventoryCount.Create("REC-2026-0001", DateOnly.FromDateTime(DateTime.Today),
            Guid.NewGuid(), null, "system");
    }

    // ── Create ────────────────────────────────────────────────────────────

    [Fact]
    public void Create_WithValidData_IsOpen()
    {
        var count = CreateOpen();

        Assert.Equal(InventoryCountStatus.Open, count.Status);
        Assert.Empty(count.Lines);
        Assert.NotEqual(Guid.Empty, count.WarehouseId);
    }

    [Fact]
    public void Create_NumberIsUppercased()
    {
        var count = InventoryCount.Create("rec-2026-0001", DateOnly.FromDateTime(DateTime.Today),
            Guid.NewGuid(), null, "system");

        Assert.Equal("REC-2026-0001", count.Number);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyNumber_Throws(string number)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            InventoryCount.Create(number, DateOnly.FromDateTime(DateTime.Today),
                Guid.NewGuid(), null, "system"));
    }

    [Fact]
    public void Create_EmptyWarehouseId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            InventoryCount.Create("REC-001", DateOnly.FromDateTime(DateTime.Today),
                Guid.Empty, null, "system"));
    }

    // ── AddLine ───────────────────────────────────────────────────────────

    [Fact]
    public void AddLine_NewItem_AddsLineWithSystemQty()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();

        count.AddLine(itemId, "ART-001", "Artículo uno", 50m, "system");

        Assert.Single(count.Lines);
        Assert.Equal(50m, count.Lines[0].SystemQuantity);
        Assert.False(count.Lines[0].IsCounted);
    }

    [Fact]
    public void AddLine_DuplicateItem_Throws()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "Artículo uno", 50m, "system");

        Assert.Throws<InvalidOperationException>(() =>
            count.AddLine(itemId, "ART-001", "Artículo uno", 50m, "system"));
    }

    [Fact]
    public void AddLine_OnClosedCount_Throws()
    {
        var count = CreateOpen();
        count.AddLine(Guid.NewGuid(), "ART-001", "A", 10m, "system");
        count.Close("system");

        Assert.Throws<InvalidOperationException>(() =>
            count.AddLine(Guid.NewGuid(), "ART-002", "B", 5m, "system"));
    }

    // ── SetCountedQuantity ────────────────────────────────────────────────

    [Fact]
    public void SetCountedQuantity_ValidLine_MarksIsCounted()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "A", 10m, "system");

        count.SetCountedQuantity(itemId, 12m, "system");

        var line = count.Lines.Single();
        Assert.True(line.IsCounted);
        Assert.Equal(12m, line.CountedQuantity);
        Assert.Equal(2m, line.Difference);
    }

    [Fact]
    public void SetCountedQuantity_NegativeValue_Throws()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "A", 10m, "system");

        Assert.Throws<ArgumentException>(() =>
            count.SetCountedQuantity(itemId, -1m, "system"));
    }

    [Fact]
    public void SetCountedQuantity_UnknownItem_Throws()
    {
        var count = CreateOpen();
        count.AddLine(Guid.NewGuid(), "ART-001", "A", 10m, "system");

        Assert.Throws<InvalidOperationException>(() =>
            count.SetCountedQuantity(Guid.NewGuid(), 5m, "system"));
    }

    [Fact]
    public void SetCountedQuantity_OnClosedCount_Throws()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "A", 10m, "system");
        count.Close("system");

        Assert.Throws<InvalidOperationException>(() =>
            count.SetCountedQuantity(itemId, 5m, "system"));
    }

    // ── Difference ────────────────────────────────────────────────────────

    [Fact]
    public void Line_Difference_IsZero_WhenNotCounted()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "A", 10m, "system");

        Assert.Equal(0m, count.Lines[0].Difference);
    }

    [Fact]
    public void Line_Difference_IsNegative_WhenCountedLessThanSystem()
    {
        var count = CreateOpen();
        var itemId = Guid.NewGuid();
        count.AddLine(itemId, "ART-001", "A", 10m, "system");
        count.SetCountedQuantity(itemId, 7m, "system");

        Assert.Equal(-3m, count.Lines[0].Difference);
    }

    // ── Close ─────────────────────────────────────────────────────────────

    [Fact]
    public void Close_FromOpen_SetsClosedStatus()
    {
        var count = CreateOpen();

        count.Close("system");

        Assert.Equal(InventoryCountStatus.Closed, count.Status);
        Assert.NotNull(count.UpdatedAt);
    }

    [Fact]
    public void Close_AlreadyClosed_Throws()
    {
        var count = CreateOpen();
        count.Close("system");

        Assert.Throws<InvalidOperationException>(() => count.Close("system"));
    }

    [Fact]
    public void Close_WithMultipleLines_AllowsUncountedLines()
    {
        var count = CreateOpen();
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        count.AddLine(id1, "ART-001", "A", 10m, "system");
        count.AddLine(id2, "ART-002", "B", 5m, "system");
        count.SetCountedQuantity(id1, 10m, "system");
        // id2 no contado

        var ex = Record.Exception(() => count.Close("system"));

        Assert.Null(ex);
        Assert.Equal(InventoryCountStatus.Closed, count.Status);
    }
}
