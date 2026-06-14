using Debales.Application.Common;
using Debales.Application.Documents;
using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Domain.Documents;
using NSubstitute;

namespace Debales.Application.Tests.Documents;

public sealed class CreateDocumentHandlerTests
{
    private readonly IDocumentRepository _documents = Substitute.For<IDocumentRepository>();
    private readonly IDocumentTypeRepository _types = Substitute.For<IDocumentTypeRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateDocumentHandler _handler;

    private static readonly Guid TypeId = Guid.NewGuid();

    public CreateDocumentHandlerTests()
    {
        _handler = new CreateDocumentHandler(_documents, _types, _unitOfWork);
    }

    private static DocumentType BuildDocType(string name = "Contrato")
    {
        var dt = DocumentType.Create(name, null, "system");
        return dt;
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesDocument()
    {
        var docType = BuildDocType("Contrato");
        _types.GetByIdAsync(TypeId, Arg.Any<CancellationToken>()).Returns(docType);

        var command = new CreateDocumentCommand(
            "Contrato 2026", null, TypeId, null, null,
            "contrato.pdf", 2048, "application/pdf", null,
            new DateTime(2026, 6, 14), "system");

        var result = await _handler.Handle(command);

        Assert.Equal("Contrato 2026", result.Title);
        Assert.Equal("Contrato", result.DocumentTypeName);
        Assert.True(result.IsActive);
        await _documents.Received(1).AddAsync(Arg.Any<Document>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentDocType_Throws()
    {
        _types.GetByIdAsync(TypeId, Arg.Any<CancellationToken>()).Returns((DocumentType?)null);

        var command = new CreateDocumentCommand(
            "Contrato", null, TypeId, null, null,
            null, null, null, null, DateTime.Today, "system");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_WithCustomerId_PropagatesCustomerId()
    {
        var docType = BuildDocType();
        var customerId = Guid.NewGuid();
        _types.GetByIdAsync(TypeId, Arg.Any<CancellationToken>()).Returns(docType);

        var command = new CreateDocumentCommand(
            "Contrato cliente", null, TypeId, customerId, null,
            null, null, null, null, DateTime.Today, "system");

        var result = await _handler.Handle(command);

        Assert.Equal(customerId, result.CustomerId);
        Assert.Null(result.SupplierId);
    }

    [Fact]
    public async Task Handle_WithSupplierId_PropagatesSupplierId()
    {
        var docType = BuildDocType();
        var supplierId = Guid.NewGuid();
        _types.GetByIdAsync(TypeId, Arg.Any<CancellationToken>()).Returns(docType);

        var command = new CreateDocumentCommand(
            "Contrato proveedor", null, TypeId, null, supplierId,
            null, null, null, null, DateTime.Today, "system");

        var result = await _handler.Handle(command);

        Assert.Equal(supplierId, result.SupplierId);
        Assert.Null(result.CustomerId);
    }
}
