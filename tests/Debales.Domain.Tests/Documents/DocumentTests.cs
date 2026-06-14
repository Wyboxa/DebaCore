using Debales.Domain.Documents;

namespace Debales.Domain.Tests.Documents;

public sealed class DocumentTests
{
    private static Guid AnyTypeId() => Guid.NewGuid();

    [Fact]
    public void Document_Create_WithValidData_Succeeds()
    {
        var typeId = AnyTypeId();
        var doc = Document.Create("Contrato 2026", null, typeId, null, null, null, null, null, null, new DateTime(2026, 6, 14, 10, 30, 0), "system");

        Assert.Equal("Contrato 2026", doc.Title);
        Assert.Equal(typeId, doc.DocumentTypeId);
        Assert.True(doc.IsActive);
        Assert.NotEqual(Guid.Empty, doc.Id);
    }

    [Fact]
    public void Document_Create_WithEmptyTitle_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Document.Create("", null, AnyTypeId(), null, null, null, null, null, null, DateTime.Today, "system"));
    }

    [Fact]
    public void Document_Create_TruncatesTimePart()
    {
        var withTime = new DateTime(2026, 6, 14, 15, 45, 22);
        var doc = Document.Create("Factura", null, AnyTypeId(), null, null, null, null, null, null, withTime, "system");

        Assert.Equal(withTime.Date, doc.DocumentDate);
        Assert.Equal(0, doc.DocumentDate.Hour);
    }

    [Fact]
    public void Document_Create_WithCustomer_AssignsCustomerId()
    {
        var customerId = Guid.NewGuid();
        var doc = Document.Create("Presupuesto", null, AnyTypeId(), customerId, null, "presupuesto.pdf", 1024, "application/pdf", null, DateTime.Today, "system");

        Assert.Equal(customerId, doc.CustomerId);
        Assert.Null(doc.SupplierId);
        Assert.Equal("presupuesto.pdf", doc.FileName);
        Assert.Equal(1024, doc.FileSizeBytes);
    }

    [Fact]
    public void Document_Update_ChangesFields()
    {
        var doc = Document.Create("Título original", null, AnyTypeId(), null, null, null, null, null, null, DateTime.Today, "system");
        var newTypeId = AnyTypeId();

        doc.Update("Título actualizado", "Nueva descripción", newTypeId, null, null, "nuevo.pdf", 2048, "application/pdf", "notas", DateTime.Today.AddDays(-1), "admin");

        Assert.Equal("Título actualizado", doc.Title);
        Assert.Equal("Nueva descripción", doc.Description);
        Assert.Equal(newTypeId, doc.DocumentTypeId);
        Assert.Equal("nuevo.pdf", doc.FileName);
        Assert.NotNull(doc.UpdatedAt);
        Assert.Equal("admin", doc.UpdatedBy);
    }

    [Fact]
    public void Document_Deactivate_SetsIsActiveFalse()
    {
        var doc = Document.Create("Documento", null, AnyTypeId(), null, null, null, null, null, null, DateTime.Today, "system");

        doc.Deactivate("admin");

        Assert.False(doc.IsActive);
        Assert.NotNull(doc.UpdatedAt);
    }

    [Fact]
    public void DocumentType_Create_WithValidData_Succeeds()
    {
        var dt = DocumentType.Create("Contrato", "Contratos firmados", "system");

        Assert.Equal("Contrato", dt.Name);
        Assert.Equal("Contratos firmados", dt.Description);
        Assert.True(dt.IsActive);
    }

    [Fact]
    public void DocumentType_Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => DocumentType.Create("", null, "system"));
    }

    [Fact]
    public void DocumentType_Deactivate_SetsIsActiveFalse()
    {
        var dt = DocumentType.Create("Factura", null, "system");
        dt.Deactivate("admin");
        Assert.False(dt.IsActive);
    }
}
