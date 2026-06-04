using Debales.Domain.Sales;

namespace Debales.Domain.Tests.Sales;

public sealed class SalesDeliveryNoteTests
{
    private static SalesDeliveryNote CreateDraftNote()
    {
        var note = SalesDeliveryNote.Create(
            "ALV-2026-0001",
            customerId: Guid.NewGuid(),
            salesOrderId: null,
            date: DateOnly.FromDateTime(DateTime.Today),
            notes: null,
            createdBy: "system");

        note.AddLine(null, null, Guid.NewGuid(), "ART-001", "Artículo 1", null, 5m);
        return note;
    }

    [Fact]
    public void Create_WithValidData_IsDraft()
    {
        var note = CreateDraftNote();

        Assert.Equal(SalesDeliveryNoteStatus.Draft, note.Status);
        Assert.Single(note.Lines);
    }

    [Fact]
    public void Post_FromDraft_SetsPostedStatus()
    {
        var note = CreateDraftNote();

        note.Post("admin");

        Assert.Equal(SalesDeliveryNoteStatus.Posted, note.Status);
        Assert.NotNull(note.UpdatedAt);
    }

    [Fact]
    public void Post_AlreadyPosted_Throws()
    {
        var note = CreateDraftNote();
        note.Post("admin");

        Assert.Throws<InvalidOperationException>(() => note.Post("admin"));
    }
}
