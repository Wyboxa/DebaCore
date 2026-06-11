using Debales.Domain.Accounting;

namespace Debales.Domain.Tests.Accounting;

public sealed class RemittanceTests
{
    private static readonly Guid _bankId = Guid.NewGuid();
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Today);

    private static Remittance CreateDraft(RemittanceType type = RemittanceType.Collection)
        => Remittance.Create("REM-C-2026-0001", _today, _bankId, type, null, "test");

    private static Remittance DraftWithOneLine()
    {
        var r = CreateDraft();
        r.AddLine(Guid.NewGuid(), 100m, "test");
        return r;
    }

    // ── Create ────────────────────────────────────────────────────────────

    [Fact]
    public void Create_WithValidData_IsDraftWithNoLines()
    {
        var r = CreateDraft();

        Assert.Equal(RemittanceStatus.Draft, r.Status);
        Assert.Equal(RemittanceType.Collection, r.Type);
        Assert.Empty(r.Lines);
        Assert.Equal(0m, r.Total);
        Assert.Equal(_bankId, r.BankAccountId);
    }

    [Fact]
    public void Create_NumberIsUppercased()
    {
        var r = Remittance.Create("rem-c-2026-0001", _today, _bankId, RemittanceType.Collection, null, "test");
        Assert.Equal("REM-C-2026-0001", r.Number);
    }

    [Fact]
    public void Create_NotesAreTrimmed()
    {
        var r = Remittance.Create("REM-P-2026-0001", _today, _bankId, RemittanceType.Payment, "  notas  ", "test");
        Assert.Equal("notas", r.Notes);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyNumber_Throws(string number)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            Remittance.Create(number, _today, _bankId, RemittanceType.Collection, null, "test"));
    }

    [Fact]
    public void Create_EmptyBankAccountId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Remittance.Create("REM-C-2026-0001", _today, Guid.Empty, RemittanceType.Collection, null, "test"));
    }

    // ── AddLine ───────────────────────────────────────────────────────────

    [Fact]
    public void AddLine_ValidData_LineAppears()
    {
        var r = CreateDraft();
        var docId = Guid.NewGuid();

        r.AddLine(docId, 250m, "test");

        Assert.Single(r.Lines);
        Assert.Equal(docId, r.Lines[0].DocumentId);
        Assert.Equal(250m, r.Lines[0].Amount);
        Assert.Equal(250m, r.Total);
    }

    [Fact]
    public void AddLine_TwoLines_TotalIsSum()
    {
        var r = CreateDraft();
        r.AddLine(Guid.NewGuid(), 100m, "test");
        r.AddLine(Guid.NewGuid(), 200m, "test");

        Assert.Equal(2, r.Lines.Count);
        Assert.Equal(300m, r.Total);
    }

    [Fact]
    public void AddLine_DuplicateDocumentId_Throws()
    {
        var r = CreateDraft();
        var docId = Guid.NewGuid();
        r.AddLine(docId, 100m, "test");

        Assert.Throws<InvalidOperationException>(() => r.AddLine(docId, 50m, "test"));
    }

    [Fact]
    public void AddLine_WhenNotDraft_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        Assert.Throws<InvalidOperationException>(() => r.AddLine(Guid.NewGuid(), 100m, "test"));
    }

    [Fact]
    public void AddLine_EmptyDocumentId_Throws()
    {
        var r = CreateDraft();

        Assert.Throws<ArgumentException>(() => r.AddLine(Guid.Empty, 100m, "test"));
    }

    // ── RemoveLine ────────────────────────────────────────────────────────

    [Fact]
    public void RemoveLine_ExistingLine_RemovesIt()
    {
        var r = CreateDraft();
        var docId = Guid.NewGuid();
        r.AddLine(docId, 100m, "test");

        r.RemoveLine(docId, "test");

        Assert.Empty(r.Lines);
        Assert.Equal(0m, r.Total);
    }

    [Fact]
    public void RemoveLine_NonExistentLine_Throws()
    {
        var r = DraftWithOneLine();

        Assert.Throws<InvalidOperationException>(() => r.RemoveLine(Guid.NewGuid(), "test"));
    }

    [Fact]
    public void RemoveLine_WhenNotDraft_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        Assert.Throws<InvalidOperationException>(() => r.RemoveLine(r.Lines[0].DocumentId, "test"));
    }

    // ── UpdateNotes ───────────────────────────────────────────────────────

    [Fact]
    public void UpdateNotes_WhenDraft_UpdatesNotes()
    {
        var r = CreateDraft();

        r.UpdateNotes("nueva nota", "test");

        Assert.Equal("nueva nota", r.Notes);
    }

    [Fact]
    public void UpdateNotes_NotesTrimmed()
    {
        var r = CreateDraft();
        r.UpdateNotes("  nota con espacios  ", "test");
        Assert.Equal("nota con espacios", r.Notes);
    }

    [Fact]
    public void UpdateNotes_WhenSent_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        Assert.Throws<InvalidOperationException>(() => r.UpdateNotes("nueva nota", "test"));
    }

    // ── Send ─────────────────────────────────────────────────────────────

    [Fact]
    public void Send_DraftWithLines_TransitionsToSent()
    {
        var r = DraftWithOneLine();

        r.Send("test");

        Assert.Equal(RemittanceStatus.Sent, r.Status);
    }

    [Fact]
    public void Send_DraftWithNoLines_Throws()
    {
        var r = CreateDraft();

        Assert.Throws<InvalidOperationException>(() => r.Send("test"));
    }

    [Fact]
    public void Send_AlreadySent_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        Assert.Throws<InvalidOperationException>(() => r.Send("test"));
    }

    [Fact]
    public void Send_Confirmed_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");
        r.Confirm("test");

        Assert.Throws<InvalidOperationException>(() => r.Send("test"));
    }

    // ── Confirm ───────────────────────────────────────────────────────────

    [Fact]
    public void Confirm_FromSent_TransitionsToConfirmed()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        r.Confirm("test");

        Assert.Equal(RemittanceStatus.Confirmed, r.Status);
    }

    [Fact]
    public void Confirm_FromDraft_Throws()
    {
        var r = DraftWithOneLine();

        Assert.Throws<InvalidOperationException>(() => r.Confirm("test"));
    }

    [Fact]
    public void Confirm_AlreadyConfirmed_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");
        r.Confirm("test");

        Assert.Throws<InvalidOperationException>(() => r.Confirm("test"));
    }

    // ── Fail ──────────────────────────────────────────────────────────────

    [Fact]
    public void Fail_FromSent_TransitionsToFailed()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        r.Fail("Rechazo bancario", "test");

        Assert.Equal(RemittanceStatus.Failed, r.Status);
        Assert.Equal("Rechazo bancario", r.FailReason);
    }

    [Fact]
    public void Fail_FromDraft_Throws()
    {
        var r = DraftWithOneLine();

        Assert.Throws<InvalidOperationException>(() => r.Fail("motivo", "test"));
    }

    [Fact]
    public void Fail_FromConfirmed_Throws()
    {
        var r = DraftWithOneLine();
        r.Send("test");
        r.Confirm("test");

        Assert.Throws<InvalidOperationException>(() => r.Fail("motivo", "test"));
    }

    [Fact]
    public void Fail_ReasonIsTrimmed()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        r.Fail("  motivo con espacios  ", "test");

        Assert.Equal("motivo con espacios", r.FailReason);
    }

    // ── Full lifecycle ─────────────────────────────────────────────────────

    [Fact]
    public void FullLifecycle_DraftToConfirmed()
    {
        var r = CreateDraft();
        r.AddLine(Guid.NewGuid(), 500m, "test");
        r.AddLine(Guid.NewGuid(), 300m, "test");

        r.Send("test");
        r.Confirm("test");

        Assert.Equal(RemittanceStatus.Confirmed, r.Status);
        Assert.Equal(2, r.Lines.Count);
        Assert.Equal(800m, r.Total);
    }

    [Fact]
    public void FullLifecycle_DraftToFailed()
    {
        var r = DraftWithOneLine();
        r.Send("test");

        r.Fail("Banco rechazó", "test");

        Assert.Equal(RemittanceStatus.Failed, r.Status);
        Assert.NotNull(r.FailReason);
    }
}
