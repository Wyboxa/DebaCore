using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Customers.Commands.ImportCustomers;
using NSubstitute;

namespace Debales.Application.Tests.Import;

public sealed class ImportCustomersHandlerTests
{
    private readonly ICustomerRepository _repo = Substitute.For<ICustomerRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ImportCustomersHandler _handler;

    public ImportCustomersHandlerTests()
    {
        _handler = new ImportCustomersHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ValidCsv_ImportsAll()
    {
        var csv = """
            Nombre,Sector,CIF,Teléfono,Email
            "Empresa A","Construcción","B12345678","912345678","a@a.es"
            "Empresa B","Retail","B87654321","934567890","b@b.es"
            """;

        var result = await _handler.Handle(new ImportCustomersCommand(csv, "test"));

        Assert.Equal(2, result.Success);
        Assert.Equal(0, result.Failed);
        Assert.Empty(result.Errors);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MissingName_SkipsRow()
    {
        var csv = """
            Nombre,Sector
            ,
            "Empresa OK","Retail"
            """;

        var result = await _handler.Handle(new ImportCustomersCommand(csv, "test"));

        Assert.Equal(1, result.Success);
        Assert.Equal(1, result.Failed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_EmptyCsv_ReturnsZero()
    {
        var csv = "Nombre,Sector,CIF\n";

        var result = await _handler.Handle(new ImportCustomersCommand(csv, "test"));

        Assert.Equal(0, result.Success);
        Assert.Equal(0, result.Failed);
        await _uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_OnlyHeader_ReturnsZero()
    {
        var csv = "Nombre,Sector,CIF,Teléfono,Email";

        var result = await _handler.Handle(new ImportCustomersCommand(csv, "test"));

        Assert.Equal(0, result.Total);
    }
}
